﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using EditorTextos.Models;
using Newtonsoft.Json;

namespace EditorTextos.Controllers
{
    public class ClientesDocumentoJuridicosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ClientesDocumentoJuridicos
        public ActionResult Index()
        {
            var clientesDocumentoJuridicos = db.ClientesDocumentoJuridicos.Include(c => c.Clientes).Include(c => c.Empresas).Include(c => c.PlantillaDocumentos);
            return View(clientesDocumentoJuridicos.ToList());
        }

        // GET: ClientesDocumentoJuridicos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientesDocumentoJuridicos clientesDocumentoJuridicos = db.ClientesDocumentoJuridicos.Find(id);
            if (clientesDocumentoJuridicos == null)
            {
                return HttpNotFound();
            }
            return View(clientesDocumentoJuridicos);
        }

        // GET: ClientesDocumentoJuridicos/Create
        public ActionResult Create()
        {
            Clientes _clientes;
            ViewBag.ClientesId = new SelectList(db.Clientes, "Id", "PrimerNombre");
            ViewBag.EmpresasId = new SelectList(db.Empresas, "Id", "Empresa");
            ViewBag.PlantillaDocumentosId = new SelectList(db.PlantillaDocumentos, "Id", "Plantilla");

            JsonSerializerSettings _settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            _clientes = db.Clientes
                .Include(d => d.Documentos.Select(t => t.TipoDocumentos))
                .Include(c => c.CorreoElectronicos.Select(t => t.TipoCorreos))
                .Include(d => d.Direcciones).Where(x => x.Id == 1).FirstOrDefault();



            var _clientesAux = db.Clientes.Where(x => x.Id == 1)
                .Include(d => d.Documentos.Select(t => t.TipoDocumentos))
                .Include(c => c.CorreoElectronicos.Select(t => t.TipoCorreos))
                .Include(d => d.Direcciones);

            var _a = _clientesAux.Select(x=> x.Documentos.Where(t=> t.TipoDocumentos.TipoDocumento=="CC"));
            //var _b=

            var _clientes2 = new List<Clientes>();
            _clientes2.Add(_clientes);

            var a = CreateDataTable<Clientes>(_clientes2);
            var b = a.Columns[0].ColumnName.ToString();
            var _c = a.Rows[0].ItemArray[5];


            List<StringJson> _stringAux = StringAux(CargarDatosClientes(_clientes));
            ViewBag._jsonDatos = JsonConvert.SerializeObject(_stringAux, _settings);
            ViewBag._clientes = _clientes;
            return View();
        }

        // POST: ClientesDocumentoJuridicos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DocumentoTexto,Resumen,ClientesId,PlantillaDocumentosId,EmpresasId,FechaCreacion,FechaActualizacion")] ClientesDocumentoJuridicos clientesDocumentoJuridicos)
        {
            if (ModelState.IsValid)
            {
                var _platilla=new PlantillaDocumentos()
                {
                    Descipcion = clientesDocumentoJuridicos.Resumen,
                    FechaActualizacion = clientesDocumentoJuridicos.FechaActualizacion,
                    FechaCreacion = clientesDocumentoJuridicos.FechaCreacion,
                    Plantilla = "PLANTILLA PRUEBA",
                    DocumentoTexto = SustituirDatos(clientesDocumentoJuridicos.DocumentoTexto)
                };
                clientesDocumentoJuridicos.ClientesId = 1;
                clientesDocumentoJuridicos.EmpresasId = 1;
                clientesDocumentoJuridicos.FechaActualizacion = DateTime.Now;
                clientesDocumentoJuridicos.FechaCreacion = DateTime.Now;
                clientesDocumentoJuridicos.PlantillaDocumentos=_platilla;
                db.ClientesDocumentoJuridicos.Add(clientesDocumentoJuridicos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientesId = new SelectList(db.Clientes, "Id", "PrimerNombre", clientesDocumentoJuridicos.ClientesId);
            ViewBag.EmpresasId = new SelectList(db.Empresas, "Id", "Empresa", clientesDocumentoJuridicos.EmpresasId);
            ViewBag.PlantillaDocumentosId = new SelectList(db.PlantillaDocumentos, "Id", "Plantilla", clientesDocumentoJuridicos.PlantillaDocumentosId);
            return View(clientesDocumentoJuridicos);
        }

        // GET: ClientesDocumentoJuridicos/Edit/5
        public ActionResult Edit(int? id)
        {
            Clientes _clientes;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientesDocumentoJuridicos clientesDocumentoJuridicos = db.ClientesDocumentoJuridicos.Find(id);
            if (clientesDocumentoJuridicos == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientesId = new SelectList(db.Clientes, "Id", "PrimerNombre", clientesDocumentoJuridicos.ClientesId);
            ViewBag.EmpresasId = new SelectList(db.Empresas, "Id", "Empresa", clientesDocumentoJuridicos.EmpresasId);
            ViewBag.PlantillaDocumentosId = new SelectList(db.PlantillaDocumentos, "Id", "Plantilla", clientesDocumentoJuridicos.PlantillaDocumentosId);

            JsonSerializerSettings _settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            _clientes = db.Clientes
                .Include(d => d.Documentos.Select(t => t.TipoDocumentos))
                .Include(c => c.CorreoElectronicos.Select(t => t.TipoCorreos))
                .Include(d => d.Direcciones).FirstOrDefault(x => x.Id == 1);
            List<StringJson> _stringAux = StringAux(CargarDatosClientes(_clientes));

            ViewBag._jsonDatos = JsonConvert.SerializeObject(_stringAux, _settings);
            ViewBag._clientes = _clientes;

            return View(clientesDocumentoJuridicos);
        }

        // POST: ClientesDocumentoJuridicos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DocumentoTexto,Resumen,ClientesId,PlantillaDocumentosId,EmpresasId,FechaCreacion,FechaActualizacion")] ClientesDocumentoJuridicos clientesDocumentoJuridicos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clientesDocumentoJuridicos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientesId = new SelectList(db.Clientes, "Id", "PrimerNombre", clientesDocumentoJuridicos.ClientesId);
            ViewBag.EmpresasId = new SelectList(db.Empresas, "Id", "Empresa", clientesDocumentoJuridicos.EmpresasId);
            ViewBag.PlantillaDocumentosId = new SelectList(db.PlantillaDocumentos, "Id", "Plantilla", clientesDocumentoJuridicos.PlantillaDocumentosId);
            return View(clientesDocumentoJuridicos);
        }

        // GET: ClientesDocumentoJuridicos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientesDocumentoJuridicos clientesDocumentoJuridicos = db.ClientesDocumentoJuridicos.Find(id);
            if (clientesDocumentoJuridicos == null)
            {
                return HttpNotFound();
            }
            return View(clientesDocumentoJuridicos);
        }

        // POST: ClientesDocumentoJuridicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClientesDocumentoJuridicos clientesDocumentoJuridicos = db.ClientesDocumentoJuridicos.Find(id);
            db.ClientesDocumentoJuridicos.Remove(clientesDocumentoJuridicos);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #region FuncionesUtiles      
        private List<StringJson> StringAux(AuxClientes clientes)
        {
            int _nroPropiedades = typeof(Clientes).GetProperties().Count();
            int i = 0;

            List<StringJson> _stringJson = new List<StringJson>()
            {
                new StringJson
                {
                    Id= i++,
                    Nombre= nameof(clientes.PrimerNombre).ToString(),
                    Descripcion= nameof(clientes.PrimerNombre).ToString() +" de "+ nameof(Clientes),
                    Valor= clientes.PrimerNombre,
                    NombrePropiedad= nameof(clientes.PrimerNombre).ToString()
                },
                new StringJson
                {
                    Id= i++,
                    Nombre= nameof(clientes.SegundoNombre).ToString(),
                    Descripcion= nameof(clientes.SegundoNombre).ToString() +" de "+ nameof(Clientes),
                    Valor= clientes.SegundoNombre,
                    NombrePropiedad= nameof(clientes.SegundoNombre).ToString()
                },
                new StringJson
                {
                    Id= i++,
                    Nombre= nameof(clientes.PrimerApellido).ToString(),
                    Descripcion= nameof(clientes.PrimerApellido).ToString() +" de "+ nameof(Clientes),
                    Valor= clientes.PrimerApellido,
                    NombrePropiedad= nameof(clientes.PrimerApellido).ToString()
                },
                new StringJson
                {
                    Id= i++,
                    Nombre= nameof(clientes.SegundoApellido).ToString(),
                    Descripcion= nameof(clientes.SegundoApellido).ToString() +" de "+ nameof(Clientes),
                    Valor= clientes.SegundoApellido,
                    NombrePropiedad= nameof(clientes.SegundoApellido).ToString()
                },
                new StringJson
                {
                    Id= i++,
                    Nombre= nameof(clientes.FechaNacimiento).ToString(),
                    Descripcion= nameof(clientes.FechaNacimiento).ToString() +" de "+ nameof(Clientes),
                    Valor= clientes.FechaNacimiento.ToString(),
                    NombrePropiedad= nameof(clientes.FechaNacimiento).ToString()
                },
                new StringJson
                {
                    Id= i++,
                    Nombre= nameof(clientes.NroDocumento).ToString(),
                    Descripcion= nameof(clientes.NroDocumento).ToString() +" de "+ nameof(Clientes),
                    Valor= clientes.NroDocumento.ToString(),
                    NombrePropiedad= nameof(clientes.NroDocumento).ToString()
                },
                new StringJson
                {
                    Id= i++,
                    Nombre= nameof(clientes.Nacionalidad).ToString(),
                    Descripcion= nameof(clientes.Nacionalidad).ToString() +" de "+ nameof(Clientes),
                    Valor= clientes.Nacionalidad,
                    NombrePropiedad= nameof(clientes.Nacionalidad).ToString()
                },
                new StringJson
                {
                    Id= i++,
                    Nombre= nameof(clientes.FechaExpedicion).ToString(),
                    Descripcion= nameof(clientes.FechaExpedicion).ToString() +" de "+ nameof(Clientes),
                    Valor= clientes.FechaExpedicion.ToString("dd-MM-yyyy"),
                    NombrePropiedad= nameof(clientes.FechaExpedicion).ToString()
                },
                new StringJson
                {
                    Id= i++,
                    Nombre= nameof(clientes.LugarExpedicion).ToString(),
                    Descripcion= nameof(clientes.LugarExpedicion).ToString() +" de "+ nameof(Clientes),
                    Valor= clientes.LugarExpedicion.ToString(),
                    NombrePropiedad= nameof(clientes.LugarExpedicion).ToString()
                },
                new StringJson
                {
                    Id= i++,
                    Nombre= nameof(clientes.TipoDocumento).ToString(),
                    Descripcion= nameof(clientes.TipoDocumento).ToString() +" de "+ nameof(Clientes),
                    Valor= clientes.TipoDocumento.ToString(),
                    NombrePropiedad= nameof(clientes.TipoDocumento).ToString()
                },
                new StringJson
                {
                    Id= i++,
                    Nombre= nameof(clientes.CorreoElectronico).ToString(),
                    Descripcion= nameof(clientes.CorreoElectronico).ToString() +" de "+ nameof(Clientes),
                    Valor= clientes.CorreoElectronico.ToString(),
                    NombrePropiedad= nameof(clientes.FechaExpedicion).ToString()
                },
                new StringJson
                {
                    Id= i++,
                    Nombre= nameof(clientes.Pais).ToString(),
                    Descripcion= nameof(clientes.Pais).ToString() +" de "+ nameof(Clientes),
                    Valor= clientes.Pais.ToString(),
                    NombrePropiedad= nameof(clientes.Pais).ToString()
                }
                ,
                new StringJson
                {
                    Id= i++,
                    Nombre= nameof(clientes.Departamento).ToString(),
                    Descripcion= nameof(clientes.Departamento).ToString() +" de "+ nameof(Clientes),
                    Valor= clientes.Departamento.ToString(),
                    NombrePropiedad= nameof(clientes.Departamento).ToString()
                }
                ,
                new StringJson
                {
                    Id= i++,
                    Nombre= nameof(clientes.Ciudad).ToString(),
                    Descripcion= nameof(clientes.Ciudad).ToString() +" de "+ nameof(Clientes),
                    Valor= clientes.Ciudad.ToString(),
                    NombrePropiedad= nameof(clientes.Ciudad).ToString()
                }
                ,
                new StringJson
                {
                    Id= i++,
                    Nombre= nameof(clientes.Direccion).ToString(),
                    Descripcion= nameof(clientes.Direccion).ToString() +" de "+ nameof(Clientes),
                    Valor= clientes.Direccion.ToString(),
                    NombrePropiedad= nameof(clientes.Direccion).ToString()
                }

            };
            return _stringJson;
        }

        private AuxClientes CargarDatosClientes(Clientes clientes)
        {
            AuxClientes _clienteAux = new AuxClientes()
            {
                Id = clientes.Id,
                PrimerNombre = clientes.PrimerNombre,
                SegundoNombre = clientes.SegundoNombre,
                PrimerApellido = clientes.PrimerApellido,
                SegundoApellido = clientes.SegundoApellido,
                FechaNacimiento = clientes.FechaNacimiento,
                TipoDocumento = clientes.Documentos.Select(t => t.TipoDocumentos).FirstOrDefault().TipoDocumento,
                NroDocumento = clientes.Documentos.Select(n => n.NroDocumento).FirstOrDefault(),
                LugarExpedicion = clientes.Documentos.Select(l => l.LugarExpedicion).FirstOrDefault(),
                FechaExpedicion = clientes.Documentos.Select(f => f.FechaExpedicion).FirstOrDefault(),
                Nacionalidad = clientes.Documentos.Select(n => n.Nacionalidad).FirstOrDefault(),
                Direccion = clientes.Direcciones.Direccion,
                Ciudad = clientes.Direcciones.Ciudad,
                Departamento = clientes.Direcciones.Departamento,
                Pais = clientes.Direcciones.Pais,
                CodeZip = clientes.Direcciones.CodeZip,
                CorreoElectronico = clientes.CorreoElectronicos.Select(c => c.CorreoElectronico).FirstOrDefault()
            };
            return _clienteAux;
        }

        private string SustituirDatos(string documentoTexto)
        {
            var clientes = db.Clientes
                .Include(d => d.Documentos.Select(t => t.TipoDocumentos))
                .Include(c => c.CorreoElectronicos.Select(t => t.TipoCorreos))
                .Include(d => d.Direcciones).FirstOrDefault(x => x.Id == 1);

            StringBuilder nuevoTexto = new StringBuilder(documentoTexto);

            nuevoTexto.Replace("[[Id]]", clientes.Id.ToString());
            nuevoTexto.Replace("[[PrimerNombre]]", clientes.PrimerNombre.ToString());
            nuevoTexto.Replace("[[SegundoNombre]]", clientes.SegundoNombre.ToString());
            nuevoTexto.Replace("[[PrimerApellido]]", clientes.PrimerApellido.ToString());
            nuevoTexto.Replace("[[SegundoApellido]]", clientes.SegundoApellido.ToString());
            nuevoTexto.Replace("[[FechaNacimiento]]", clientes.FechaNacimiento.ToString());
            nuevoTexto.Replace("[[TipoDocumento]]", clientes.Documentos.Select(t => t.TipoDocumentos).FirstOrDefault().TipoDocumento.ToString());
            nuevoTexto.Replace("[[NroDocumento]]", clientes.Documentos.Select(n => n.NroDocumento).FirstOrDefault().ToString());
            nuevoTexto.Replace("[[LugarExpedicion]]", clientes.Documentos.Select(l => l.LugarExpedicion).FirstOrDefault().ToString());
            nuevoTexto.Replace("[[FechaExpedicion]]", clientes.Documentos.Select(f => f.FechaExpedicion).FirstOrDefault().ToString());
            nuevoTexto.Replace("[[Nacionalidad]]", clientes.Documentos.Select(n => n.Nacionalidad).FirstOrDefault().ToString());
            nuevoTexto.Replace("[[Direccion]]", clientes.Direcciones.Direccion.ToString());
            nuevoTexto.Replace("[[Ciudad]]", clientes.Direcciones.Ciudad.ToString());
            nuevoTexto.Replace("[[Departamento]]", clientes.Direcciones.Departamento.ToString());
            nuevoTexto.Replace("[[Pais]]", clientes.Direcciones.Pais.ToString());
            nuevoTexto.Replace("[[CodeZip]]", clientes.Direcciones.CodeZip.ToString());
            nuevoTexto.Replace("[[CorreoElectronico]]", clientes.CorreoElectronicos.Select(c => c.CorreoElectronico).FirstOrDefault().ToString());

            return nuevoTexto.ToString();
        }


/// <summary>
/// Create data table from list.
/// https://stackoverflow.com/questions/18746064/using-reflection-to-create-a-datatable-from-a-class
/// </summary>
        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();
            DataTable dataTable = new DataTable();
            int j = 0;

            foreach (PropertyInfo info in properties)
            {
                j++;
                //if (typeof(T).GetProperty(info.Name).GetGetMethod().IsVirtual )
                //{
                //    //if (info.PropertyType.IsGenericType)
                //    //{
                //    //    dataTable.Columns.AddRange(_createDateTable(info));
                //    //}
                //}
                //else
                //{
                    dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
                //}                
            }
            
            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    //if (typeof(IEnumerable).IsAssignableFrom(entity.GetType().GetTypeInfo()))
                    //{

                    //}
                    //else
                    //{
                        values[i] = properties[i].GetValue(entity);
                    //}                    
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        private static DataColumn[] _createDateTable(PropertyInfo info)
        {




            throw new NotImplementedException();
        }

        private string _buscarTipoDocumento(List<TipoDocumentos> _tipoDocumentos)
        {
            foreach (var item in _tipoDocumentos)
            {
                if (item.TipoDocumento == "CC")
                {
                    return item.TipoDocumento.ToString();
                }
            }

            return "";
        }
        #endregion
    }
}
