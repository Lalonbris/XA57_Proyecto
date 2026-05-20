import { useState, useEffect } from 'react';

export function useConfiguradorData(productoId) {
    const [data, setData] = useState({
        modelos: [],
        lineas: [],
        producto: null,
        loading: true,
        error: null
    });

    useEffect(() => {
        async function fetchData() {
            try {
                const [modelosRes, lineasRes, productoRes] = await Promise.all([
                    fetch('/api/configurador/modelos'),
                    fetch('/api/configurador/lineas'),
                    fetch(`/api/configurador/producto/${productoId}`)
                ]);

                if (!modelosRes.ok || !lineasRes.ok || !productoRes.ok) {
                    throw new Error('Error al cargar datos del servidor');
                }

                const [modelos, lineas, producto] = await Promise.all([
                    modelosRes.json(),
                    lineasRes.json(),
                    productoRes.json()
                ]);

                setData({
                    modelos,
                    lineas,
                    producto,
                    loading: false,
                    error: null
                });
            } catch (err) {
                setData(prev => ({
                    ...prev,
                    loading: false,
                    error: err.message || 'Error de conexión'
                }));
            }
        }

        if (productoId) {
            fetchData();
        }
    }, [productoId]);

    return data;
}
