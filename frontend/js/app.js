(function () {
  'use strict';

  angular.module('mesaDeAyuda', [])
    .constant('API_URL', 'http://localhost:5080/api')
    .factory('IncidenciasService', IncidenciasService)
    .controller('IncidenciasController', IncidenciasController);

  IncidenciasService.$inject = ['$http', 'API_URL'];
  function IncidenciasService($http, API_URL) {
    var base = API_URL + '/incidencias';
    return {
      listar: function (filtros) {
        var params = {};
        if (filtros.estado) params.estado = filtros.estado;
        if (filtros.prioridad) params.prioridad = filtros.prioridad;
        return $http.get(base, { params: params }).then(function (r) { return r.data; });
      },
      crear: function (incidencia) {
        return $http.post(base, incidencia).then(function (r) { return r.data; });
      },
      cambiarEstado: function (id, estado) {
        return $http.patch(base + '/' + id + '/estado', { estado: estado });
      },
      asignar: function (id, tecnicoId) {
        return $http.patch(base + '/' + id + '/asignar', { tecnicoId: tecnicoId });
      },
      eliminar: function (id) {
        return $http.delete(base + '/' + id);
      },
      tecnicos: function () {
        return $http.get(API_URL + '/usuarios/tecnicos').then(function (r) { return r.data; });
      }
    };
  }

  IncidenciasController.$inject = ['IncidenciasService'];
  function IncidenciasController(IncidenciasService) {
    var vm = this;

    vm.estados = ['Abierta', 'EnProgreso', 'Resuelta', 'Cerrada'];
    vm.prioridades = ['Baja', 'Media', 'Alta', 'Critica'];
    vm.filtros = { estado: '', prioridad: '' };
    vm.incidencias = [];
    vm.tecnicos = [];
    vm.nueva = nuevaIncidencia();

    vm.cargar = cargar;
    vm.crear = crear;
    vm.cambiarEstado = cambiarEstado;
    vm.asignar = asignar;
    vm.eliminar = eliminar;
    vm.etiquetaEstado = etiquetaEstado;
    vm.claseEstado = claseEstado;
    vm.clasePrioridad = clasePrioridad;

    IncidenciasService.tecnicos().then(function (t) { vm.tecnicos = t; }, mostrarError);
    cargar();

    function nuevaIncidencia() {
      return { titulo: '', descripcion: '', prioridad: 'Media' };
    }

    function cargar() {
      vm.cargando = true;
      IncidenciasService.listar(vm.filtros)
        .then(function (data) { vm.incidencias = data; vm.error = null; }, mostrarError)
        .finally(function () { vm.cargando = false; });
    }

    function crear() {
      vm.guardando = true;
      IncidenciasService.crear(vm.nueva)
        .then(function () {
          vm.nueva = nuevaIncidencia();
          vm.form.$setPristine();
          cargar();
        }, mostrarError)
        .finally(function () { vm.guardando = false; });
    }

    function cambiarEstado(incidencia) {
      IncidenciasService.cambiarEstado(incidencia.id, incidencia.estado).then(cargar, function (e) {
        mostrarError(e);
        cargar();
      });
    }

    function asignar(incidencia) {
      IncidenciasService.asignar(incidencia.id, incidencia.tecnicoAsignadoId || null).then(cargar, function (e) {
        mostrarError(e);
        cargar();
      });
    }

    function eliminar(incidencia) {
      if (!confirm('¿Eliminar la incidencia #' + incidencia.id + '?')) return;
      IncidenciasService.eliminar(incidencia.id).then(cargar, mostrarError);
    }

    function mostrarError(respuesta) {
      vm.error = (respuesta && respuesta.data && respuesta.data.mensaje) ||
        'No se pudo conectar con la API. Verificá que esté corriendo en ' + 'http://localhost:5080.';
    }

    function etiquetaEstado(estado) {
      return estado === 'EnProgreso' ? 'En progreso' : estado;
    }

    function claseEstado(estado) {
      return {
        Abierta: 'text-bg-secondary',
        EnProgreso: 'text-bg-info',
        Resuelta: 'text-bg-success',
        Cerrada: 'text-bg-dark'
      }[estado];
    }

    function clasePrioridad(prioridad) {
      return {
        Baja: 'text-bg-light border',
        Media: 'text-bg-primary',
        Alta: 'text-bg-warning',
        Critica: 'text-bg-danger'
      }[prioridad];
    }
  }
})();
