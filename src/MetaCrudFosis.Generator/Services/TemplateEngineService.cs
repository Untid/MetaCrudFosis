using System.Text;

namespace MetaCrudFosis.Generator.Services;

public record Campo(string Name, string Type);

public class TemplateEngineService
{
    private readonly string _templatesDir;
    public TemplateEngineService(string templatesDir) => _templatesDir = templatesDir;

    private string Leer(string nombre) => File.ReadAllText(Path.Combine(_templatesDir, nombre));
    private static string CamelCase(string s) => char.ToLowerInvariant(s[0]) + s.Substring(1);
    private static string ValorDefecto(string tipo) => tipo == "string" ? " = string.Empty;" : "";

    public string GenerarModeloApi(string entity, List<Campo> campos)
    {
        var props = new StringBuilder();
        foreach (var c in campos)
            props.AppendLine($"    public {c.Type} {c.Name} {{ get; set; }}{ValorDefecto(c.Type)}");
        return Leer("ModelApi.tt")
            .Replace("<#= EntityName #>", entity)
            .Replace("<#= Properties #>", props.ToString().TrimEnd());
    }

    public string GenerarModeloWeb(string entity, List<Campo> campos)
    {
        var props = new StringBuilder();
        foreach (var c in campos)
        {
            props.AppendLine($"    [Display(Name = \"{c.Name}\")]");
            if (c.Type == "DateTime")
            {
                props.AppendLine("    [DataType(DataType.Date)]");
                props.AppendLine("    [DisplayFormat(DataFormatString = \"{0:yyyy-MM-dd}\", ApplyFormatInEditMode = false)]");
            }
            props.AppendLine($"    public {c.Type} {c.Name} {{ get; set; }}{ValorDefecto(c.Type)}");
            props.AppendLine();
        }
        return Leer("ModelWeb.tt")
            .Replace("<#= EntityName #>", entity)
            .Replace("<#= PropertiesWeb #>", props.ToString().TrimEnd());
    }

    public string GenerarControladorWeb(string entity)
        => Leer("ControllerWeb.tt").Replace("<#= EntityName #>", entity);

    public string GenerarVistaIndex(string entity, string corpColor, List<Campo> campos)
    {
        var filterOptions = new StringBuilder();
        var tableHeaders = new StringBuilder();
        var rowCells = new StringBuilder();
        var bulkCeldas = new StringBuilder();
        var bulkObjeto = new StringBuilder();
        var ordenColumnas = new List<string>();

        for (int i = 0; i < campos.Count; i++)
        {
            var c = campos[i];
            var js = CamelCase(c.Name);
            ordenColumnas.Add(c.Name);

            filterOptions.AppendLine($"            <option value=\"{c.Name}\">{c.Name}</option>");
            tableHeaders.Append($"<th>{c.Name}</th>");

            var celda = c.Type switch
            {
                "DateTime" => $"${{fmtFecha(x.{js})}}",
                "decimal" or "int" => $"${{fmtNum(x.{js})}}",
                "bool" => $"${{x.{js} ? 'Sí' : 'No'}}",
                _ => $"${{x.{js} ?? ''}}"
            };
            rowCells.AppendLine($"                    <td>{celda}</td>");

            // Mapeo desde celdas de texto (orden fijo): convierte según tipo
            var conv = c.Type switch
            {
                "int" => $"parseInt(c[{i}]) || 0",
                "decimal" => $"parseFloat((c[{i}] || '').replace(',', '.')) || 0",
                "bool" => $"['true','sí','si','1'].includes((c[{i}] || '').toLowerCase())",
                "DateTime" => $"c[{i}] || new Date().toISOString().slice(0,10)",
                _ => $"c[{i}] || ''"
            };
            bulkCeldas.AppendLine($"                {js}: {conv},");

            // Mapeo desde objeto JSON (acepta camelCase y PascalCase)
            var convObj = c.Type switch
            {
                "int" => $"parseInt(o.{js} ?? o.{c.Name} ?? 0) || 0",
                "decimal" => $"Number(o.{js} ?? o.{c.Name} ?? 0)",
                "bool" => $"Boolean(o.{js} ?? o.{c.Name} ?? false)",
                "DateTime" => $"(o.{js} ?? o.{c.Name} ?? new Date().toISOString()).slice(0,10)",
                _ => $"o.{js} ?? o.{c.Name} ?? ''"
            };
            bulkObjeto.AppendLine($"                {js}: {convObj},");
        }

        return Leer("ViewIndex.tt")
            .Replace("<#= EntityName #>", entity)
            .Replace("<#= CorpColor #>", corpColor)
            .Replace("<#= FilterOptions #>", filterOptions.ToString().TrimEnd())
            .Replace("<#= TableHeaders #>", tableHeaders.ToString())
            .Replace("<#= RowCells #>", rowCells.ToString().TrimEnd())
            .Replace("<#= OrdenColumnas #>", string.Join(" · ", ordenColumnas))
            .Replace("<#= NumCampos #>", campos.Count.ToString())
            .Replace("<#= BulkMapCeldas #>", bulkCeldas.ToString().TrimEnd())
            .Replace("<#= BulkMapObjeto #>", bulkObjeto.ToString().TrimEnd());
    }
    public string GenerarVistaCreate(string entity, string corpColor, List<Campo> campos)
            => GenerarFormulario("ViewCreate.tt", entity, corpColor, campos);

    public string GenerarVistaEdit(string entity, string corpColor, List<Campo> campos)
        => GenerarFormulario("ViewEdit.tt", entity, corpColor, campos);

    private string GenerarFormulario(string plantilla, string entity, string corpColor, List<Campo> campos)
    {
        var formFields = new StringBuilder();
        foreach (var c in campos)
        {
            formFields.AppendLine("        <div class=\"form-group\">");
            formFields.AppendLine($"            <label asp-for=\"{c.Name}\"></label>");
            var input = c.Type switch
            {
                "int" => $"<input asp-for=\"{c.Name}\" type=\"number\" />",
                "decimal" => $"<input asp-for=\"{c.Name}\" type=\"number\" step=\"0.01\" />",
                "bool" => $"<input asp-for=\"{c.Name}\" type=\"checkbox\" />",
                "DateTime" => $"<input asp-for=\"{c.Name}\" />",
                _ => $"<input asp-for=\"{c.Name}\" />"
            };
            formFields.AppendLine($"            {input}");
            formFields.AppendLine($"            <span asp-validation-for=\"{c.Name}\"></span>");
            formFields.AppendLine("        </div>");
        }

        return Leer(plantilla)
            .Replace("<#= EntityName #>", entity)
            .Replace("<#= CorpColor #>", corpColor)
            .Replace("<#= FormFields #>", formFields.ToString().TrimEnd());
    }
}