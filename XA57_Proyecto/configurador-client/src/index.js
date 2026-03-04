import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";

const contenedor = document.getElementById("configurador-root");
if (contenedor) {
    const productoId = contenedor.dataset.productoId;
    const productoImagen = contenedor.dataset.productoImagen;
    const root = ReactDOM.createRoot(contenedor);
    root.render(<App productoId={productoId} productoImagen={productoImagen} />);
}