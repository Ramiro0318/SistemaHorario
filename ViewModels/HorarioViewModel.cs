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
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using System.Windows.Input;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace SistemaHorario.ViewModels
{
    public enum Tipos { Clase, Actividad };
    public enum Ventanas { AgregarActividad, Editar, Horario, Eliminar };
    public enum Dia { Domingo, Lunes, Martes, Miercoles, Jueves, Viernes, Sabado };
    public class HorarioViewModel : INotifyPropertyChanged
    {
        public ActividadRespository repositoryAct = new();
        public ClaseRepository repositoryClase = new();

        public ObservableCollection<Object> Domingo { get; set; } = new();
        public ObservableCollection<Object> Lunes { get; set; } = new();
        public ObservableCollection<Object> Martes { get; set; } = new();
        public ObservableCollection<Object> Miercoles { get; set; } = new();
        public ObservableCollection<Object> Jueves { get; set; } = new();
        public ObservableCollection<Object> Viernes { get; set; } = new();
        public ObservableCollection<Object> Sabado { get; set; } = new();
        public Actividad? Actividad { get; set; }
        public Clase? Clase { get; set; }
        public Object? Object { get; set; }
        public string Error { get; set; } = "";
        public IEnumerable<Dia> ListaDias => Enum.GetValues(typeof(Dia)).Cast<Dia>();
        public Dia Dia { get; set; } = Dia.Domingo;

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
            VerEliminarCommand = new RelayCommand(VerEliminar);
            EliminarCommand = new RelayCommand(Eliminar);
            IrEditarCommand = new RelayCommand<Object>(IrEditar);
            EditarCommand = new RelayCommand(Editar);
            CancelarCommand = new RelayCommand(Cancelar);

            Cargar();
        }


        public bool ClaseSeleccionada => TipoSeleccionado == Tipos.Clase;
        public bool ActividadSeleccionada => TipoSeleccionado == Tipos.Actividad;


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
                    Validar(Actividad);
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
                    Validar(Clase);
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

        private void VerEliminar()
        {
            Ventana = Ventanas.Eliminar;
            Actualizar(nameof(Ventana));
        }

        private void Eliminar()
        {
            if (Object != null)
            {
                var Tipo = Object.GetType();
                if (Tipo.Name == "Actividad")
                {
                    TipoSeleccionado = Tipos.Actividad;
                    Actividad = (Actividad?)Object;
                    repositoryAct.Delete(Actividad);
                }
                if (Tipo.Name == "Clase")
                {
                    TipoSeleccionado = Tipos.Clase;
                    Clase = (Clase?)Object;
                    repositoryClase.Delete(Clase);
                }
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
            Domingo.Clear();
            Lunes.Clear();
            Martes.Clear();
            Miercoles.Clear();
            Jueves.Clear();
            Viernes.Clear();
            Sabado.Clear(); 

            var domingoAct = repositoryAct.GetDay("Domingo");
            foreach (var item in domingoAct) {
                Domingo.Add(item);
            }
            var domingoClase = repositoryClase.GetDay("Domingo");
            foreach (var item in domingoClase) {
                Domingo.Add(item);
            }

            var lunesAct = repositoryAct.GetDay("Lunes");
            foreach (var item in lunesAct) {
                Lunes.Add(item);
            }
            var lunesClase = repositoryClase.GetDay("Lunes");
            foreach (var item in lunesClase) {
                Lunes.Add(item);
            }

            var martesAct = repositoryAct.GetDay("Martes");
            foreach (var item in martesAct) {
                Martes.Add(item);
            }
            var martesClase = repositoryClase.GetDay("Martes");
            foreach (var item in martesClase) {
                Martes.Add(item);
            }

            var miercolesAct = repositoryAct.GetDay("Miercoles");
            foreach (var item in miercolesAct) {
                Miercoles.Add(item);
            }
            var miercolesClase = repositoryClase.GetDay("Miercoles");
            foreach (var item in miercolesClase) {
                Miercoles.Add(item);
            }

            var juevesAct = repositoryAct.GetDay("Jueves");
            foreach (var item in juevesAct) {
                Jueves.Add(item);
            }
            var juevesClase = repositoryClase.GetDay("Jueves");
            foreach (var item in juevesClase) {
                Jueves.Add(item);
            }

            var viernesAct = repositoryAct.GetDay("Viernes");
            foreach (var item in viernesAct) {
                Viernes.Add(item);
            }
            var viernesClase = repositoryClase.GetDay("Viernes");

            foreach (var item in viernesClase) {
                Viernes.Add(item);
            }
            var sabadoAct = repositoryAct.GetDay("Sabado");
            foreach (var item in sabadoAct) {
                Sabado.Add(item);
            }
            var sabadoClase = repositoryClase.GetDay("Sabado");
            foreach (var item in sabadoClase) { 
                Sabado.Add(item); 
            }

        }

        private void Validar(Actividad Actividad)
        {
            TimeOnly HoraInicio = TimeOnly.ParseExact(Actividad.HoraInicio, "H:mm", CultureInfo.InvariantCulture);
            TimeOnly HoraFin = TimeOnly.ParseExact(Actividad.HoraFin, "H:mm", CultureInfo.InvariantCulture);

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
            {
                Error += "\n Indique correctamente la hora";
            }
            if (string.IsNullOrWhiteSpace(Actividad.Descripcion))
            {
                Error += "\n Añada una descripción a la actividad";
            }
            bool ActEmpalmada = repositoryAct.GetDay(Actividad.Dia).Any(x =>
            (HoraInicio < TimeOnly.ParseExact(x.HoraInicio, "H:mm") && HoraFin > TimeOnly.ParseExact(x.HoraInicio, "H:mm") ) ||
             HoraInicio <  TimeOnly.ParseExact(x.HoraFin, "H:mm")    && HoraFin > TimeOnly.ParseExact(x.HoraFin, "H:mm") ||
             HoraInicio > TimeOnly.ParseExact(x.HoraInicio, "H:mm") && HoraFin < TimeOnly.ParseExact(x.HoraFin, "H:mm")  ||
             HoraInicio < TimeOnly.ParseExact(x.HoraInicio, "H:mm") && HoraFin > TimeOnly.ParseExact(x.HoraFin, "H:mm") );
            
            bool ActEmpalmada2 = repositoryClase.GetDay(Actividad.Dia).Any(x =>
            (HoraInicio < TimeOnly.ParseExact(x.HoraInicio, "H:mm") && HoraFin > TimeOnly.ParseExact(x.HoraInicio, "H:mm")) ||
             HoraInicio <  TimeOnly.ParseExact(x.HoraFin, "H:mm") && HoraFin >   TimeOnly.ParseExact(x.HoraFin, "H:mm") ||
             HoraInicio >  TimeOnly.ParseExact(x.HoraInicio, "H:mm") && HoraFin < TimeOnly.ParseExact(x.HoraFin, "H:mm") ||
             HoraInicio <  TimeOnly.ParseExact(x.HoraInicio, "H:mm") && HoraFin > TimeOnly.ParseExact(x.HoraFin, "H:mm"));
            if (ActEmpalmada || ActEmpalmada2)
            {
                Error += "\n Esta hora se encuentra ocupada";
            }
        }

        private void Validar(Clase Clase)
        {
            TimeOnly HoraInicio = TimeOnly.ParseExact(Clase.HoraInicio, "H:mm", CultureInfo.InvariantCulture);
            TimeOnly HoraFin = TimeOnly.ParseExact(Clase.HoraFin, "H:mm", CultureInfo.InvariantCulture);

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
            {
                Error += "\n Indique correctamente la hora";
            }
            if (string.IsNullOrWhiteSpace(Clase.Asignatura) || string.IsNullOrWhiteSpace(Clase.Maestro)
                || string.IsNullOrWhiteSpace(Clase.Aula))
            {
                Error += "\n Indique correctamente la información de la clase";
            }
            bool ClaseEmpalmada = repositoryAct.GetDay(Clase.Dia).Any(x =>
            (HoraInicio < TimeOnly.ParseExact(x.HoraInicio, "H:mm") && HoraFin > TimeOnly.ParseExact(x.HoraInicio, "H:mm")) ||
             HoraInicio < TimeOnly.ParseExact(x.HoraFin, "H:mm") && HoraFin > TimeOnly.ParseExact(x.HoraFin, "H:mm") ||
             HoraInicio > TimeOnly.ParseExact(x.HoraInicio, "H:mm") && HoraFin < TimeOnly.ParseExact(x.HoraFin, "H:mm") ||
             HoraInicio < TimeOnly.ParseExact(x.HoraInicio, "H:mm") && HoraFin > TimeOnly.ParseExact(x.HoraFin, "H:mm"));

            bool ClaseEmpalmada2 = repositoryClase.GetDay(Clase.Dia).Any(x =>
            (HoraInicio < TimeOnly.ParseExact(x.HoraInicio, "H:mm") && HoraFin > TimeOnly.ParseExact(x.HoraInicio, "H:mm")) ||
             HoraInicio <  TimeOnly.ParseExact(x.HoraFin, "H:mm") && HoraFin >    TimeOnly.ParseExact(x.HoraFin, "H:mm") ||
             HoraInicio >  TimeOnly.ParseExact(x.HoraInicio, "H:mm") && HoraFin < TimeOnly.ParseExact(x.HoraFin, "H:mm") ||
             HoraInicio <  TimeOnly.ParseExact(x.HoraInicio, "H:mm") && HoraFin > TimeOnly.ParseExact(x.HoraFin, "H:mm"));
            if (ClaseEmpalmada || ClaseEmpalmada2)
            {
                Error += "\n Esta hora se encuentra ocupada";
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
