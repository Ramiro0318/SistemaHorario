using CommunityToolkit.Mvvm.Input;
using SistemaHorario.Models;
using SistemaHorario.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace SistemaHorario.ViewModels
{
    public enum Tipos
    {
        Clase, Actividad
    };
    public enum Ventanas
    {
        AgregarActividad, Editar, Horario, Eliminar
    };
    public enum Dia { dom, lun, mar, mie, jue, vie, sab };
    public class HorarioViewModel : INotifyPropertyChanged
    {
        private ActividadRespository repositoryAct = new();
        private ClaseRepository repositoryClase = new();

        public ObservableCollection<Actividad> Horario { get; set; } = new();
        public ObservableCollection<Clase> Horario2 { get; set; } = new();
        public ObservableCollection<Object> Horario3 { get; set; } = new();
        public Actividad? Actividad { get; set; }
        public Clase? Clase { get; set; }
        public Object? Object { get; set; }

        public string Error { get; set; } = "";

        public IEnumerable<Dia> ListaDias => Enum.GetValues(typeof(Dia)).Cast<Dia>();
        public Dia Dia { get; set; } = Dia.dom;
        public IEnumerable<Tipos> ListaT { get; } = new List<Tipos> { Tipos.Clase, Tipos.Actividad };
        private Tipos tipoSeleccionado;
        public Tipos TipoSeleccionado
        {
            get => tipoSeleccionado;
            set
            {
                tipoSeleccionado = value;
                Actualizar(null);
            }
        }

        //public ObservableCollection<string> Horas { get; } = new ObservableCollection<string>
        //{ "0:00 - 0:59",
        //  "1:00 - 1:59",
        //  "2:00 - 2:59",
        //  "3:00 - 3:59",
        //  "4:00 - 4:59",
        //  "5:00 - 5:59",
        //  "6:00 - 6:59",
        //  "7:00 - 7:59",
        //  "8:00 - 8:59",
        //  "9:00 - 9:59",
        //  "10:00 - 10:59",
        //  "11:00 - 11:59",
        //  "12:00 - 12:59",
        //  "13:00 - 13:59",
        //  "14:00 - 14:59",
        //  "15:00 - 15:59",
        //  "16:00 - 16:59",
        //  "17:00 - 17:59",
        //  "18:00 - 18:59",
        //  "19:00 - 19:59",
        //  "20:00 - 20:59",
        //  "21:00 - 21:59",
        //  "22:00 - 22:59",
        //  "23:00 - 23:59",
        //};

        public Ventanas Ventana { get; set; } = Ventanas.Horario;
        public ICommand IrAgregarCommand { get; set; }
        public ICommand AgregarCommand { get; set; }
        public ICommand VerEliminarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand IrEditarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        

        public HorarioViewModel()
        {
            IrAgregarCommand = new RelayCommand(IrAgregar);
            AgregarCommand = new RelayCommand(Agregar);
            VerEliminarCommand = new RelayCommand<Actividad>(VerEliminar);
            EliminarCommand = new RelayCommand(Eliminar);
            IrEditarCommand = new RelayCommand<Actividad>(IrEditar);
            EditarCommand = new RelayCommand(Editar);
            CancelarCommand = new RelayCommand(Cancelar);

            Cargar();
        }


        public bool ClaseSeleccionada => TipoSeleccionado == Tipos.Clase;
        public bool ActividadSeleccionada => TipoSeleccionado == Tipos.Actividad;


        //private void IrEditar(Actividad? a)
        //{
        //    a = Actividad;
        //    if (a != null)
        //    {
        //        Actividad = new Actividad
        //        {
        //            Id = a.Id,
        //            Nombre = a.Nombre,
        //            Dia = a.Dia,
        //            HoraInicio = a.HoraInicio,
        //            HoraFin = a.HoraFin,
        //            Descripcion = a.Descripcion,

        //        };
        //        Actualizar(nameof(Actividad));

        //        if (Actividad.Descripcion == null)
        //        { TipoSeleccionado = Tipos.Clase; }
        //        else { TipoSeleccionado = Tipos.Actividad; }
        //        Actualizar(nameof(TipoSeleccionado));

        //        Ventana = Ventanas.Editar;
        //        Actualizar(nameof(Ventana));
        //    }
        //}


        private void IrEditar(Object? o)
        {
            if (Object != null)
            {
                var Tipo = Object.GetType();
                if (Tipo.Name == "Actividad")
                {
                    TipoSeleccionado = Tipos.Actividad;
                    Actividad = (Actividad?)Object;
                    Actividad = new Actividad
                    {
                        Id = Actividad.Id,
                        Nombre = Actividad.Nombre,
                        Dia = Actividad.Dia,
                        HoraInicio = Actividad.HoraInicio,
                        HoraFin = Actividad.HoraFin,
                        Descripcion = Actividad.Descripcion,

                    };
                    Actualizar(nameof(Actividad));
                }
                if (Tipo.Name == "Clase")
                {
                    TipoSeleccionado = Tipos.Clase;
                    Clase = (Clase?)Object;
                    Clase = new Clase
                    {
                        Id = Clase.Id,
                        Nombre = Clase.Nombre,
                        Dia = Clase.Dia,
                        HoraInicio = Clase.HoraInicio,
                        HoraFin = Clase.HoraFin,
                        Aula = Clase.Aula,
                        Asignatura = Clase.Asignatura,
                        Maestro = Clase.Maestro,

                    };
                    Actualizar(nameof(Clase));

                }

                //if (Actividad.Descripcion == null)
                //{ TipoSeleccionado = Tipos.Clase; }
                //else { TipoSeleccionado = Tipos.Actividad; }
                //Actualizar(nameof(TipoSeleccionado));


                Ventana = Ventanas.Editar;
                Actualizar(nameof(Ventana));
            }
           
        }

        private void Editar()
        {
            if (TipoSeleccionado == Tipos.Actividad)
            {
                if (Actividad != null)
                {
                    //ValidarAct();
                    Actualizar(nameof(Error));

                    if (Error == "")
                    {
                        repositoryAct.Update(Actividad);
                        Cancelar();
                        Cargar();
                    }
                }
            }
            else
            {
                if (Clase != null)
                {
                    //Validar !
                    Actualizar(nameof(Error));

                    if (Error == "")
                    {
                        repositoryClase.Update(Clase);
                        Cancelar();
                        Cargar();
                    }
                }

            }
        }

        private void VerEliminar(Actividad? actividad)
        {
            actividad = Actividad;
            Actualizar(nameof(actividad));

            Ventana = Ventanas.Eliminar;
            Actualizar(nameof(Ventana));
        }

        private void Eliminar()
        {
            if (Actividad != null)
            {
                repositoryAct.Delete(Actividad);
                Cancelar();
            }
        }

        private void Cancelar()
        {
            Error = "";
            Actualizar(nameof(Error));

            Cargar();
            Ventana = Ventanas.Horario;
            Actualizar(nameof(Ventana));
        }
        

        private void Cargar()
        {
            Horario.Clear();
            Horario2.Clear();
            Horario3.Clear();

            var a = repositoryAct.GetAll();
            foreach (var item in a)
            {
                Horario3.Add(item);
            }
            var b = repositoryClase.GetAll();
            foreach (var item in b)
            {
                Horario3.Add(item);
            }
        }

        private void Validar(Actividad Actividad)
        {
            if (string.IsNullOrWhiteSpace(Actividad.Nombre))
            {
                Error += "Indique el nombre de la actividad";
            }
            if (string.IsNullOrWhiteSpace(Actividad.Dia))
            {
                Error += "\n Indique el día en que se realiza la actividad";
            }
            if (Actividad.HoraInicio == null || Actividad.HoraFin == null ||
                TimeOnly.ParseExact(Actividad.HoraInicio, "H:mm", CultureInfo.InvariantCulture) >=
                TimeOnly.ParseExact(Actividad.HoraFin, "H:mm", CultureInfo.InvariantCulture))
            //Actividad.HoraFin || Actividad.HoraInicio == Actividad.HoraFin)
            {
                Error += "\n Indique correctamente la hora";
            }
            if (string.IsNullOrWhiteSpace(Actividad.Descripcion))
            {
                Error += "\n Añada una descripción a la actividad";
            }
        }
        private void Validar(Clase Clase)
        {
            if (string.IsNullOrWhiteSpace(Clase.Nombre))
            {
                Error += "Indique el nombre de la clase";
            }
            if (string.IsNullOrWhiteSpace(Clase.Dia))
            {
                Error += "\n Indique el día en que día se tiene la clase";
            }
            if (Clase.HoraInicio == null || Clase.HoraFin == null ||
                TimeOnly.ParseExact(Clase.HoraInicio, "H:mm", CultureInfo.InvariantCulture) >=
                TimeOnly.ParseExact(Clase.HoraFin, "H:mm", CultureInfo.InvariantCulture))
            //Actividad.HoraFin || Actividad.HoraInicio == Actividad.HoraFin)
            {
                Error += "\n Indique correctamente la hora";
            }
            if (string.IsNullOrWhiteSpace(Clase.Asignatura) || string.IsNullOrWhiteSpace(Clase.Maestro) 
                || string.IsNullOrWhiteSpace(Clase.Aula)) {

                Error += "\n Indique correctamente la información de la clase";
            }
        }

        private void Agregar()
        {
            if (TipoSeleccionado == Tipos.Actividad)
            {
                if (Actividad != null)
                {
                    Validar(Actividad);
                    Actualizar(nameof(Error));

                    if (Error == "")
                    {
                        repositoryAct.Insert(Actividad);
                        Cancelar();
                    }
                }
            }

            if (TipoSeleccionado == Tipos.Clase)
            {
                if (Clase != null)
                {
                    Validar(Clase);
                    Actualizar(nameof(Error));

                    if (Error == "")
                    {
                        repositoryClase.Insert(Clase);
                        Cancelar();
                    }
                }
            }
        }

        private void IrAgregar()
        {
            Actividad = new();
            Clase = new();
            Ventana = Ventanas.AgregarActividad;
            Actualizar(nameof(Ventana));
        }

        private void Actualizar(string? nombre = null)
        {
            PropertyChanged?.Invoke(this, new(nombre));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
