import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";

console.log("XA57: React App script loaded. Attempting to mount...");
window.addEventListener('DOMContentLoaded', () => {
    const contenedor = document.getElementById("configurador-root");
    if (contenedor) {
        console.log("XA57: Found #configurador-root. Rendering React app.");
        const root = ReactDOM.createRoot(contenedor);
        root.render(<App datasetProps={contenedor.dataset} />);
    } else {
        console.error("XA57: Could not find #configurador-root element.");
    }
});