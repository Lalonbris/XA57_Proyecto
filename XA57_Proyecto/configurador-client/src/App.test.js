import { render, screen } from '@testing-library/react';
import App from './App';

jest.mock('./hooks/useConfiguradorData', () => ({
    useConfiguradorData: () => ({
        modelos: [
            { id: 1, nombre: 'Irizar i8', fabricante: 'Irizar' },
            { id: 2, nombre: 'Volvo 9800', fabricante: 'Volvo' }
        ],
        lineas: [
            { id: 1, nombre: 'ETN', colorPrimario: '#FF0000', colorSecundario: '#FFFFFF' },
            { id: 2, nombre: 'Omnibus de Mexico', colorPrimario: '#003087', colorSecundario: '#C8A800' }
        ],
        producto: {
            id: 5,
            nombre: 'Busito de Peluche',
            precio: 350,
            tipoProducto: {
                id: 1,
                nombre: 'Busito de Peluche',
                maxCaracteres: 25,
                permiteNombre: true,
                permiteNumeroEconomico: true,
                permiteRuta: true
            }
        },
        loading: false,
        error: null
    })
}));

// Suppress React 19 act() warnings in test environment
const originalError = console.error;
beforeAll(() => {
    console.error = (...args) => {
        if (/act|inside a test/i.test(args[0])) return;
        originalError.call(console, ...args);
    };
});
afterAll(() => {
    console.error = originalError;
});

describe('App (Configurador)', () => {
    test('renderiza el nombre del producto', () => {
        render(<App />);
        expect(screen.getByText('Busito de Peluche')).toBeInTheDocument();
    });

    test('renderiza el precio del producto', () => {
        render(<App />);
        expect(screen.getByText('$350.00')).toBeInTheDocument();
    });

    test('renderiza el titulo de personalizacion', () => {
        render(<App />);
        expect(screen.getByText('Personalización del Producto')).toBeInTheDocument();
    });

    test('renderiza modelos de autobus', () => {
        render(<App />);
        expect(screen.getByText('Irizar i8')).toBeInTheDocument();
        expect(screen.getByText('Volvo 9800')).toBeInTheDocument();
    });

    test('renderiza lineas cromaticas', () => {
        render(<App />);
        expect(screen.getByText('ETN')).toBeInTheDocument();
        expect(screen.getByText('Omnibus de Mexico')).toBeInTheDocument();
    });

    test('muestra el boton de agregar al carrito', () => {
        render(<App />);
        expect(screen.getByText('Agregar al carrito')).toBeInTheDocument();
    });

    test('muestra el control de cantidad con valor 1', () => {
        render(<App />);
        expect(screen.getByText('1')).toBeInTheDocument();
    });
});
