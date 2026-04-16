import { useState, useEffect } from "react";

/**
 * Custom hook para obtener datos del configurador desde la API
 * Implementa llamadas AJAX en paralelo como se especifica en EPIC-07-03
 */
export const useConfiguradorData = (productoId) => {
  const [modelos, setModelos] = useState([]);
  const [lineas, setLineas] = useState([]);
  const [producto, setProducto] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        setError(null);
        
        // Llamadas en paralelo como se especifica en EPIC-07-03
        const [modelosResponse, lineasResponse, productoResponse] = await Promise.all([
          fetch(`/api/configurador/modelos`),
          fetch(`/api/configurador/lineas`),
          fetch(`/api/configurador/producto/${productoId}`)
        ]);

        // Verificar que todas las respuestas sean exitosas
        if (!modelosResponse.ok) throw new Error(`Error al obtener modelos: ${modelosResponse.status}`);
        if (!lineasResponse.ok) throw new Error(`Error al obtener líneas: ${lineasResponse.status}`);
        if (!productoResponse.ok) throw new Error(`Error al obtener producto: ${productoResponse.status}`);

        // Parsear JSON
        const modelosData = await modelosResponse.json();
        const lineasData = await lineasResponse.json();
        const productoData = await productoResponse.json();

        // Actualizar estado
        setModelos(modelosData);
        setLineas(lineasData);
        setProducto(productoData);
      } catch (err) {
        setError(err.message);
        console.error("Error fetching configurator data:", err);
      } finally {
        setLoading(false);
      }
    };

    if (productoId) {
      fetchData();
    }
  }, [productoId]);

  return { modelos, lineas, producto, loading, error };
};