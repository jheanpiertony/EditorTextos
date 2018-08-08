using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace EditorTextos.Models
{
    public class Entidad
    {
        [Key]
        public int Id { get; set; }
    }

    [Table("Clientes")]
    public class Clientes: Entidad
    {
        public String PrimerNombre { get; set; }
        public String SegundoNombre { get; set; }
        public String PrimerApellido { get; set; }
        public String SegundoApellido { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaNacimiento { get; set; }

        public virtual ICollection<ClientesDocumentoJuridicos> ClientesDocumentoJuridicos { get; set; }
        public int DireccionesId { get; set; }
        public virtual Direcciones Direcciones { get; set; }
        public virtual ICollection<Documentos> Documentos { get; set; }
        public virtual ICollection<CorreoElectronicos> CorreoElectronicos { get; set; }
    }

    [Table("Empresas")]
    public class Empresas : Entidad
    {
        public String Empresa { get; set; }

        public int DireccionesId { get; set; }
        public virtual Direcciones Direcciones { get; set; }
        public int CorreoElectronicosId { get; set; }
        public virtual CorreoElectronicos CorreoElectronicos { get; set; }
        public virtual List<ClientesDocumentoJuridicos> ClientesDocumentoJuridicos { get; set; }
    }

    [Table("Documentos")]
    public class Documentos: Entidad
    {
        public Int64 NroDocumento { get; set; }
        public string Nacionalidad { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaExpedicion { get; set; }
        public string LugarExpedicion { get; set; }

        public int ClientesId { get; set; }
        public virtual Clientes Clientes { get; set; }
        public int TipoDocumentosId { get; set; }
        public virtual TipoDocumentos TipoDocumentos { get; set; }
    }

    [Table("TipoDocumentos")]
    public class TipoDocumentos: Entidad
    {
        public string TipoDocumento { get; set; }
        public string Descripcion { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Prioridad { get; set; }
        //public virtual Documentos  Documentos{ get; set; }
    }

    [Table("CorreoElectronicos")]
    public class CorreoElectronicos: Entidad
    {
        [Required(ErrorMessage = "Este campo es obligatorio."), DataType(DataType.EmailAddress),
            RegularExpression(@"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$")]
        public string CorreoElectronico { get; set; }

        public int? ClientesId { get; set; }
        public virtual Clientes Clientes { get; set; }
        public int TipoCorreosId { get; set; }
        public virtual TipoCorreos TipoCorreos { get; set; }
    }

    [Table("TipoCorreos")]
    public class TipoCorreos: Entidad
    {
        public string TipoCorreo { get; set; }
    }

    [Table("Direcciones")]
    public class Direcciones : Entidad
    {
        public string Pais { get; set; }
        public string Departamento { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        public string CodeZip { get; set; }
    }

    [Table("PlantillaDocumentos")]
    public class PlantillaDocumentos : EditarActulizar
    {
        public string Plantilla { get; set; }
        public string Descipcion { get; set; }
        [AllowHtml]
        public string DocumentoTexto { get; set; }

        public virtual ICollection<ClientesDocumentoJuridicos> ClientesDocumentoJuridicos { get; set; }
    }

    [Table("ClientesDocumentoJuridicos")]
    public class ClientesDocumentoJuridicos : EditarActulizar
    {
        [AllowHtml]
        public string DocumentoTexto { get; set; }
        public string Resumen { get; set; }

        public virtual Clientes Clientes { get; set; }
        public virtual PlantillaDocumentos PlantillaDocumentos { get; set; }
        public virtual Empresas Empresas { get; set; }
        public int ClientesId { get; set; }
        public int PlantillaDocumentosId { get; set; }
        public int EmpresasId { get; set; }
    }

    public class EditarActulizar : Entidad
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaCreacion { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaActualizacion { get; set; }
    }

    public class AuxClientes : Entidad
    {
        public String PrimerNombre { get; set; }
        public String SegundoNombre { get; set; }
        public String PrimerApellido { get; set; }
        public String SegundoApellido { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaNacimiento { get; set; }

        public Int64 NroDocumento { get; set; }
        public string Nacionalidad { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaExpedicion { get; set; }
        public string LugarExpedicion { get; set; }

        public string TipoDocumento { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio."), DataType(DataType.EmailAddress),
            RegularExpression(@"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$")]
        public string CorreoElectronico { get; set; }

        public string Pais { get; set; }
        public string Departamento { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        public string CodeZip { get; set; }
    }

    class StringJson
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Nombre { get; set; }
        [JsonProperty(PropertyName = "value")]
        public string Valor { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Descripcion { get; set; }
        [JsonProperty(PropertyName = "porpertyname")]
        public string NombrePropiedad { get; set; }
    }

}