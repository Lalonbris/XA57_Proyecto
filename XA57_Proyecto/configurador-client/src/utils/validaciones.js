/**
 * Utilidades de validación para el configurador
 * Cumple con EPIC-07-09 y especifica validaciones RN-02, RN-03, RN-04
 * También cumple con 06-api-contracts.md §5
 */

/**
 * Valida la configuración completa según las reglas de negocio
 * @param {Object} config - Configuración a validar
 * @param {Object} tipoProducto - Información del tipo de producto con sus flags y límites
 * @returns {Object} - Objeto con errores (vacío si no hay errores)
 */
export const validarConfiguracion = (config, tipoProducto) => {
  const errores = {};

  // RN-02: Modelo y Línea obligatorios
  if (!config.modeloAutobusId) {
    errores.modelo = "El modelo es obligatorio";
  }
  if (!config.lineaId) {
    errores.linea = "La línea / cromática es obligatoria";
  }

  // RN-03: Límite de caracteres (solo si el campo está habilitado y tiene valor)
  if (tipoProducto.permiteNombre && config.nombreOperador) {
    if (config.nombreOperador.length > tipoProducto.maxCaracteres) {
      errores.nombreOperador = `Máximo ${tipoProducto.maxCaracteres} caracteres`;
    }
  }
  
  if (tipoProducto.permiteNumeroEconomico && config.numeroEconomico) {
    if (config.numeroEconomico.length > tipoProducto.maxCaracteres) {
      errores.numeroEconomico = `Máximo ${tipoProducto.maxCaracteres} caracteres`;
    }
  }
  
  if (tipoProducto.permiteRuta && config.ruta) {
    if (config.ruta.length > tipoProducto.maxCaracteres) {
      errores.ruta = `Máximo ${tipoProducto.maxCaracteres} caracteres`;
    }
  }
  
  if (config.notasEspeciales) {
    // Usamos el mismo límite que el tipoProducto para consistencia
    const maxLength = tipoProducto.maxCaracteres || 20;
    if (config.notasEspeciales.length > maxLength) {
      errores.notasEspeciales = `Máximo ${maxLength} caracteres`;
    }
  }

  // RN-04: Caracteres permitidos
  // Solo se permiten letras, números y caracteres básicos: áéíóúÁÉÍÓÚñÑüÜ\s\-\,\/\#
  const regex = /^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑüÜ\s\-\,\/\#]*$/;
  
  const camposTexto = ['nombreOperador', 'numeroEconomico', 'ruta', 'notasEspeciales'];
  camposTexto.forEach(campo => {
    if (config[campo] && config[campo].trim() !== '') {
      if (!regex.test(config[campo])) {
        errores[campo] = 'Solo se permiten letras, números y caracteres básicos';
      }
    }
  });

  return errores; // {} = sin errores
};

/**
 * Valida que un valor no exceda el límite máximo de caracteres
 * @param {string} valor - Valor a validar
 * @param {number} max - Longitud máxima permitida
 * @returns {string|null} - Mensaje de error o null si es válido
 */
export const validarLongitud = (valor, max) => {
  if (valor && valor.length > max) {
    return `Máximo ${max} caracteres`;
  }
  return null;
};

/**
 * Valida que un valor contenga solo caracteres permitidos
 * @param {string} valor - Valor a validar
 * @returns {string|null} - Mensaje de error o null si es válido
 */
export const validarCaracteres = (valor) => {
  if (!valor) return null;
  
  const regex = /^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑüÜ\s\-\,\/\#]*$/;
  if (!regex.test(valor)) {
    return 'Solo se permiten letras, números y caracteres básicos';
  }
  return null;
};