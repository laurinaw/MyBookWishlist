window.toggleMenu = function () {
    const menu = document.getElementById('navMenu');
    menu.classList.toggle('open');
};

window.setupNavMenuClose = function () {
    const menu = document.getElementById('navMenu');
    const toggleButton = document.getElementById('burgernav');

    document.addEventListener('click', function (event) {
        if (
            menu.classList.contains('open') &&
            !menu.contains(event.target) &&
            !toggleButton.contains(event.target)
        ) {
            menu.classList.remove('open');
        }
    });

    const links = menu.querySelectorAll('.nav-link');
    links.forEach(link => {
        link.addEventListener('click', () => {
            menu.classList.remove('open');
        });
    });
};