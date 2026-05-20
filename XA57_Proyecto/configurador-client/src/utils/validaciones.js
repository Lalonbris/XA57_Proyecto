export function validarConfiguracion(config, tipoProducto) {
    const errores = {};
    
    // RN-02: Modelo y Línea obligatorios
    if (!config.modeloAutobusId) {
        errores.modelo = 'El modelo de autobús es obligatorio';
    }
    
    if (!config.lineaId) {
        errores.linea = 'La línea / cromática es obligatoria';
    }
    
    if (tipoProducto) {
        // RN-03: Límite de caracteres
        const max = tipoProducto.maxCaracteres || 20;
        
        if (tipoProducto.permiteNombre && config.nombreOperador?.length > max) {
            errores.nombreOperador = `El nombre no debe exceder los ${max} caracteres`;
        }
        
        if (tipoProducto.permiteNumeroEconomico && config.numeroEconomico?.length > max) {
            errores.numeroEconomico = `El número económico no debe exceder los ${max} caracteres`;
        }
        
        if (tipoProducto.permiteRuta && config.ruta?.length > max) {
            errores.ruta = `La ruta no debe exceder los ${max} caracteres`;
        }
    }

    // RN-04: Caracteres permitidos
    // Solo se permiten letras, números, espacios y caracteres básicos: áéíóúÁÉÍÓÚñÑüÜ - . , / #
    const regex = /^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑüÜ\s\-\.\,\/\#]*$/;
    
    const camposTexto = [
        { key: 'nombreOperador', label: 'Nombre del operador' },
        { key: 'numeroEconomico', label: 'Número económico' },
        { key: 'ruta', label: 'Ruta' },
        { key: 'notasEspeciales', label: 'Notas especiales' }
    ];

    camposTexto.forEach(campo => {
        const valor = config[campo.key];
        if (valor && !regex.test(valor)) {
            errores[campo.key] = `El campo '${campo.label}' contiene caracteres no permitidos`;
        }
    });

    return errores; // Objeto vacío si no hay errores
}
