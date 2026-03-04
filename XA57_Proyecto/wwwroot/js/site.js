document.addEventListener('DOMContentLoaded', function () {
    
    // Cambiar color del corazón al hacer clic (Favoritos)
    const favoriteBtns = document.querySelectorAll('.btn-favorite');
    favoriteBtns.forEach(btn => {
        btn.addEventListener('click', function(e) {
            e.preventDefault(); // Evita que recargue si estuviera en un enlace
            const icon = this.querySelector('.material-icons');
            if(icon.innerText === 'favorite_border') {
                icon.innerText = 'favorite';
                icon.style.color = '#ef4444'; // Rojo
            } else {
                icon.innerText = 'favorite_border';
                icon.style.color = ''; // Vuelve al color original
            }
        });
    });

    // Filtros de Tipo de Artículo visualmente activos
    const filterTags = document.querySelectorAll('.filter-tag');
    filterTags.forEach(tag => {
        tag.addEventListener('click', function() {
            // Quita la clase active de todos
            filterTags.forEach(t => t.classList.remove('active'));
            // Se la pone al clickeado
            this.classList.add('active');
        });
    });
});