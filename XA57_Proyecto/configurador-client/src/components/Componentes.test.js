import { render, screen, fireEvent } from '@testing-library/react';
import ModeloSelector from './ModeloSelector';
import LineaSelector from './LineaSelector';
import CantidadControl from './CantidadControl';
import PersonalizacionForm from './PersonalizacionForm';

const t = {
    bg0: "#070707", bg1: "#0f0f0f", bg2: "#161616", bg3: "#1d1d1d",
    bg4: "#252525", bg5: "#2e2e2e", bd1: "#232323", bd2: "#313131",
    bd3: "#424242", tx0: "#f5f5f5", tx1: "#c4c4c4", tx2: "#878787",
    tx3: "#525252", sv1: "#d8d8d8", white: "#ffffff",
    ffH: "'Space Grotesk', system-ui, sans-serif",
    ffB: "'Inter', system-ui, sans-serif",
};

describe('ModeloSelector', () => {
    const modelos = [
        { id: 1, nombre: 'Irizar i8', fabricante: 'Irizar' },
        { id: 2, nombre: 'Volvo 9800', fabricante: 'Volvo' }
    ];
    const onSelect = jest.fn();

    beforeEach(() => { onSelect.mockClear(); });

    test('renderiza todos los modelos', () => {
        render(<ModeloSelector modelos={modelos} selectedId={null} onSelect={onSelect} error={null} t={t} />);
        expect(screen.getByText('Irizar i8')).toBeInTheDocument();
        expect(screen.getByText('Volvo 9800')).toBeInTheDocument();
    });

    test('muestra etiqueta de obligatorio', () => {
        render(<ModeloSelector modelos={modelos} selectedId={null} onSelect={onSelect} error={null} t={t} />);
        expect(screen.getByText(/obligatorio/i)).toBeInTheDocument();
    });

    test('llama onSelect al hacer click en un modelo', () => {
        render(<ModeloSelector modelos={modelos} selectedId={null} onSelect={onSelect} error={null} t={t} />);
        fireEvent.click(screen.getByText('Irizar i8'));
        expect(onSelect).toHaveBeenCalledWith(1);
    });

    test('muestra mensaje de error cuando existe', () => {
        const errorMsg = 'El modelo de autobús es obligatorio';
        render(<ModeloSelector modelos={modelos} selectedId={null} onSelect={onSelect} error={errorMsg} t={t} />);
        expect(screen.getByText(errorMsg)).toBeInTheDocument();
    });
});

describe('LineaSelector', () => {
    const lineas = [
        { id: 1, nombre: 'ETN', colorPrimario: '#FF0000', colorSecundario: '#FFFFFF' },
        { id: 2, nombre: 'Omnibus', colorPrimario: '#003087', colorSecundario: '#C8A800' }
    ];
    const onSelect = jest.fn();

    beforeEach(() => { onSelect.mockClear(); });

    test('renderiza todas las lineas', () => {
        render(<LineaSelector lineas={lineas} selectedLinea={null} onSelect={onSelect} error={null} t={t} />);
        expect(screen.getByText('ETN')).toBeInTheDocument();
        expect(screen.getByText('Omnibus')).toBeInTheDocument();
    });

    test('llama onSelect con el objeto linea al hacer click', () => {
        render(<LineaSelector lineas={lineas} selectedLinea={null} onSelect={onSelect} error={null} t={t} />);
        fireEvent.click(screen.getByText('ETN'));
        expect(onSelect).toHaveBeenCalledWith(lineas[0]);
    });

    test('muestra mensaje de error', () => {
        const errorMsg = 'La línea / cromática es obligatoria';
        render(<LineaSelector lineas={lineas} selectedLinea={null} onSelect={onSelect} error={errorMsg} t={t} />);
        expect(screen.getByText(errorMsg)).toBeInTheDocument();
    });
});

describe('CantidadControl', () => {
    const onChange = jest.fn();

    beforeEach(() => { onChange.mockClear(); });

    test('renderiza la cantidad actual', () => {
        render(<CantidadControl cantidad={3} onChange={onChange} t={t} />);
        expect(screen.getByText('3')).toBeInTheDocument();
    });

    test('incrementa cantidad al hacer click en +', () => {
        render(<CantidadControl cantidad={1} onChange={onChange} t={t} />);
        fireEvent.click(screen.getByText('+'));
        expect(onChange).toHaveBeenCalledWith(2);
    });

    test('decrementa cantidad al hacer click en -', () => {
        render(<CantidadControl cantidad={3} onChange={onChange} t={t} />);
        fireEvent.click(screen.getByText('\u2212'));
        expect(onChange).toHaveBeenCalledWith(2);
    });

    test('no permite cantidad menor a 1', () => {
        render(<CantidadControl cantidad={1} onChange={onChange} t={t} />);
        fireEvent.click(screen.getByText('\u2212'));
        expect(onChange).toHaveBeenCalledWith(1);
    });
});

describe('PersonalizacionForm', () => {
    const tipoProducto = {
        id: 1,
        nombre: 'Busito de Peluche',
        maxCaracteres: 25,
        permiteNombre: true,
        permiteNumeroEconomico: true,
        permiteRuta: true
    };
    const valores = {
        nombreOperador: '',
        numeroEconomico: '',
        ruta: '',
        notasEspeciales: ''
    };
    const onChange = jest.fn();

    beforeEach(() => { onChange.mockClear(); });

    test('muestra campos de texto cuando tipoProducto tiene flags', () => {
        render(<PersonalizacionForm tipoProducto={tipoProducto} valores={valores} onChange={onChange} errores={{}} t={t} />);
        expect(screen.getByText('Nombre del Operador')).toBeInTheDocument();
        expect(screen.getByText('Número Económico')).toBeInTheDocument();
        expect(screen.getByText('Ruta / Destino')).toBeInTheDocument();
    });

    test('no muestra campo nombre si PermiteNombre es false', () => {
        const tipoSinNombre = { ...tipoProducto, permiteNombre: false };
        render(<PersonalizacionForm tipoProducto={tipoSinNombre} valores={valores} onChange={onChange} errores={{}} t={t} />);
        expect(screen.queryByText('Nombre del Operador')).toBeNull();
    });

    test('muestra errores de validacion', () => {
        const errores = { nombreOperador: 'Caracteres no permitidos' };
        render(<PersonalizacionForm tipoProducto={tipoProducto} valores={valores} onChange={onChange} errores={errores} t={t} />);
        expect(screen.getByText('Caracteres no permitidos')).toBeInTheDocument();
    });

    test('no renderiza nada si tipoProducto es null', () => {
        const { container } = render(
            <PersonalizacionForm tipoProducto={null} valores={valores} onChange={onChange} errores={{}} t={t} />
        );
        expect(container.innerHTML).toBe('');
    });
});
