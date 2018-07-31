namespace EditorTextos.Migrations
{
    using EditorTextos.Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.Globalization;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            context.Clientes.AddOrUpdate(
               x => x.Id,
               new Clientes { Id = 1, PrimerNombre = "Antonio", SegundoNombre = "David", PrimerApellido = "Restrepo", SegundoApellido = "Carvajal", FechaNacimiento = new DateTime(1982, 12, 18), DireccionesId = 1 },
               new Clientes { Id = 2, PrimerNombre = "Martin", SegundoNombre = "Olmedo", PrimerApellido = "Gonzalez", SegundoApellido = "Perez", FechaNacimiento = new DateTime(1980, 02, 10), DireccionesId = 2 }
               );

            context.Documentos.AddOrUpdate(
               x => x.Id,
               new Documentos { Id = 1, TipoDocumentosId = 1, ClientesId = 1, FechaExpedicion = new DateTime(2010, 07, 1), LugarExpedicion = "Bogota", Nacionalidad = "Colombiano", NroDocumento = 1126905946 },
               new Documentos { Id = 2, TipoDocumentosId = 4, ClientesId = 2, FechaExpedicion = new DateTime(2017, 01, 31), LugarExpedicion = "Maturin", Nacionalidad = "Venezolano", NroDocumento = 876543290 }
               );

            context.TipoDocumentos.AddOrUpdate(
               x => x.Id,
               new TipoDocumentos { Id = 1, TipoDocumento = "CC", Descripcion = "cedula de ciudadania", Prioridad = 1 },
               new TipoDocumentos { Id = 2, TipoDocumento = "TI", Descripcion = "tarjeta de identidad", Prioridad = 2 },
               new TipoDocumentos { Id = 3, TipoDocumento = "CE", Descripcion = "cedula de extrajero", Prioridad = 3 },
               new TipoDocumentos { Id = 4, TipoDocumento = "PS", Descripcion = "pasaporte", Prioridad = 4 },
               new TipoDocumentos { Id = 5, TipoDocumento = "PEP", Descripcion = "permiso especial de permanencia", Prioridad = 5 }
               );

            context.CorreoElectronicos.AddOrUpdate(
               x => x.Id,
               new CorreoElectronicos { Id = 1, CorreoElectronico = "desarrollador@unicoc.edu.co", TipoCorreosId = 1, ClientesId = 1 },
               new CorreoElectronicos { Id = 2, CorreoElectronico = "viejotin@hotmail.com", TipoCorreosId = 2, ClientesId = 2 }, 
               new CorreoElectronicos { Id = 3, CorreoElectronico = "ford@ford.co", TipoCorreosId = 1  },
               new CorreoElectronicos { Id = 4, CorreoElectronico = "unicoc@unicoc.edu.co", TipoCorreosId = 1 }
               );

            context.TipoCorreos.AddOrUpdate(
               x => x.Id,
               new TipoCorreos { Id = 1, TipoCorreo = "Correo institucional" },
               new TipoCorreos { Id = 2, TipoCorreo = "Correo personal" },
               new TipoCorreos { Id = 3, TipoCorreo = "Correo secundario" }
               );

            context.Direcciones.AddOrUpdate(
               x => x.Id,
               new Direcciones { Id = 1, Ciudad = "Bogota", Departamento = "Cundinamarca", Direccion = "Carrera 3 #10-00, Apto. 404", Pais = "Colombia", CodeZip = "2006-1" },
               new Direcciones { Id = 2, Ciudad = "Valledupar", Departamento = "Cesar", Direccion = "Calle 27 #16-20", Pais = "Colombia", CodeZip = "2910-0" }
               );

            context.PlantillaDocumentos.AddOrUpdate(
               x => x.Id,
               new PlantillaDocumentos { Id = 1, Plantilla = "Plantilla prueba 1", Descipcion = "Plantilla de prueba", DocumentoTexto = "<p>Prueba nro 1</p>", FechaCreacion = new DateTime(2018, 01, 01), FechaActualizacion = new DateTime(2018, 01, 01) },
               new PlantillaDocumentos { Id = 2, Plantilla = "Plantilla prueba 2", Descipcion = "Plantilla de prueba", DocumentoTexto = "<p>Prueba nro 2</p>", FechaCreacion = new DateTime(2018, 01, 01), FechaActualizacion = new DateTime(2018, 01, 01) }
               );

            context.Empresas.AddOrUpdate(
               x => x.Id,
               new Empresas { Id = 1, Empresa = "FORD",  CorreoElectronicosId= 3, DireccionesId = 1 },
               new Empresas { Id = 2, Empresa = "UNICOC", CorreoElectronicosId = 4, DireccionesId = 2 }
               );

        }
    }
}
