import { validarConfiguracion } from '../utils/validaciones';

describe('validarConfiguracion', () => {
    const configValida = {
        modeloAutobusId: 1,
        lineaId: 1,
        nombreOperador: 'Juan Pérez',
        numeroEconomico: '105',
        ruta: 'México - Guadalajara',
        notasEspeciales: ''
    };

    const tipoProducto = {
        maxCaracteres: 25,
        permiteNombre: true,
        permiteNumeroEconomico: true,
        permiteRuta: true
    };

    // ============ RN-02: Modelo y Línea obligatorios ============

    test('retorna error si modelo no seleccionado', () => {
        const config = { ...configValida, modeloAutobusId: null };
        const errores = validarConfiguracion(config, tipoProducto);
        expect(errores.modelo).toBeDefined();
        expect(errores.modelo).toMatch(/modelo.*obligatorio/i);
    });

    test('retorna error si línea no seleccionada', () => {
        const config = { ...configValida, lineaId: null };
        const errores = validarConfiguracion(config, tipoProducto);
        expect(errores.linea).toBeDefined();
        expect(errores.linea).toMatch(/línea.*cromática.*obligatoria/i);
    });

    test('no retorna errores si modelo y línea están presentes', () => {
        const errores = validarConfiguracion(configValida, tipoProducto);
        expect(errores.modelo).toBeUndefined();
        expect(errores.linea).toBeUndefined();
    });

    // ============ RN-03: Límite de caracteres ============

    test('retorna error si nombreOperador excede maxCaracteres', () => {
        const config = { ...configValida, nombreOperador: 'A'.repeat(26) };
        const errores = validarConfiguracion(config, tipoProducto);
        expect(errores.nombreOperador).toBeDefined();
        expect(errores.nombreOperador).toMatch(/25/);
    });

    test('retorna error si numeroEconomico excede maxCaracteres', () => {
        const config = { ...configValida, numeroEconomico: '1'.repeat(26) };
        const errores = validarConfiguracion(config, tipoProducto);
        expect(errores.numeroEconomico).toBeDefined();
        expect(errores.numeroEconomico).toMatch(/25/);
    });

    test('retorna error si ruta excede maxCaracteres', () => {
        const config = { ...configValida, ruta: 'X'.repeat(26) };
        const errores = validarConfiguracion(config, tipoProducto);
        expect(errores.ruta).toBeDefined();
        expect(errores.ruta).toMatch(/25/);
    });

    test('acepta texto justo en el límite de maxCaracteres', () => {
        const config = { ...configValida, nombreOperador: 'A'.repeat(25) };
        const errores = validarConfiguracion(config, tipoProducto);
        expect(errores.nombreOperador).toBeUndefined();
    });

    test('no valida campos si el flag del tipoProducto es false', () => {
        const tipoSinNombre = { ...tipoProducto, permiteNombre: false };
        const config = { ...configValida, nombreOperador: 'A'.repeat(100) };
        const errores = validarConfiguracion(config, tipoSinNombre);
        expect(errores.nombreOperador).toBeUndefined();
    });

    test('usa maxCaracteres por defecto 20 si no está definido', () => {
        const tipoSinMax = { permiteNombre: true, permiteNumeroEconomico: false, permiteRuta: false };
        const config = { ...configValida, nombreOperador: 'A'.repeat(21) };
        const errores = validarConfiguracion(config, tipoSinMax);
        expect(errores.nombreOperador).toBeDefined();
        expect(errores.nombreOperador).toContain('20');
    });

    // ============ RN-04: Caracteres permitidos ============

    test('rechaza caracteres especiales en nombreOperador', () => {
        const config = { ...configValida, nombreOperador: 'Juan<script>alert(1)</script>' };
        const errores = validarConfiguracion(config, tipoProducto);
        expect(errores.nombreOperador).toBeDefined();
        expect(errores.nombreOperador).toMatch(/caracteres no permitidos/i);
    });

    test('rechaza caracteres especiales en numeroEconomico', () => {
        const config = { ...configValida, numeroEconomico: '105<evil>' };
        const errores = validarConfiguracion(config, tipoProducto);
        expect(errores.numeroEconomico).toBeDefined();
        expect(errores.numeroEconomico).toMatch(/caracteres no permitidos/i);
    });

    test('rechaza caracteres especiales en ruta', () => {
        const config = { ...configValida, ruta: 'CDMX|GDL' };
        const errores = validarConfiguracion(config, tipoProducto);
        expect(errores.ruta).toBeDefined();
        expect(errores.ruta).toMatch(/caracteres no permitidos/i);
    });

    test('rechaza caracteres especiales en notasEspeciales', () => {
        const config = { ...configValida, notasEspeciales: 'Test%injection' };
        const errores = validarConfiguracion(config, tipoProducto);
        expect(errores.notasEspeciales).toBeDefined();
        expect(errores.notasEspeciales).toMatch(/caracteres no permitidos/i);
    });

    test('acepta tildes, ñ, ü y caracteres válidos', () => {
        const config = {
            ...configValida,
            nombreOperador: 'José García-López',
            ruta: 'México - Guadalajara'
        };
        const errores = validarConfiguracion(config, tipoProducto);
        expect(Object.keys(errores).length).toBe(0);
    });

    test('acepta números, puntos, comas, / y #', () => {
        const config = {
            ...configValida,
            nombreOperador: 'Operador #1, S.A. de C.V.',
            numeroEconomico: '105/A',
            ruta: 'CDMX/Guadalajara'
        };
        const errores = validarConfiguracion(config, tipoProducto);
        expect(Object.keys(errores).length).toBe(0);
    });

    // ============ Campos opcionales vacíos ============

    test('no valida regex en campos vacíos', () => {
        const config = { ...configValida, nombreOperador: '', ruta: '' };
        const errores = validarConfiguracion(config, tipoProducto);
        expect(errores.nombreOperador).toBeUndefined();
        expect(errores.ruta).toBeUndefined();
    });

    test('no valida regex en campos nulos', () => {
        const config = { ...configValida, nombreOperador: undefined, notasEspeciales: null };
        const errores = validarConfiguracion(config, tipoProducto);
        expect(errores.nombreOperador).toBeUndefined();
        expect(errores.notasEspeciales).toBeUndefined();
    });

    // ============ Sin tipoProducto ============

    test('funciona sin tipoProducto (solo valida modelo y línea)', () => {
        const config = { ...configValida, nombreOperador: 'script' };
        const errores = validarConfiguracion(config, null);
        expect(errores.modelo).toBeUndefined();
        expect(errores.linea).toBeUndefined();
        expect(errores.nombreOperador).toBeUndefined();
    });

    // ============ RN-05: Integridad del texto ============
    // (Se verifica indirectamente: el texto no se modifica durante la validación)

    test('el texto no es modificado por la validación', () => {
        const nombreOriginal = 'Expreso Futura';
        const config = { ...configValida, nombreOperador: nombreOriginal };
        const errores = validarConfiguracion(config, tipoProducto);
        expect(errores.nombreOperador).toBeUndefined();
        expect(config.nombreOperador).toBe(nombreOriginal);
    });
});
