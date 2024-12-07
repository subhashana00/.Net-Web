document.addEventListener("scroll", function () {
    var img = document.querySelector('.animated-img');
    var imgPosition = img.getBoundingClientRect().top;
    var screenPosition = window.innerHeight / 1.3;

    // Show image when scrolling down
    if (imgPosition < screenPosition) {
        img.classList.add('scrolled');
    }
        // Hide image when scrolling back up
    else {
        img.classList.remove('scrolled');
    }
});



document.addEventListener('DOMContentLoaded', function () {
    const section = document.getElementById('title-section');
    const images = [
        '../Images/ingredients-near-pizza.jpg',
        '../Images/grilled-meat-skewers-chicken-shish-kebab-with-zucchini-tomatoes-red-onions.jpg',
        '../Images/homemade-meal-cooked-poultry-fresh-vegetable-celebration-generated-by-ai.jpg',
        '../Images/top-view-delicious-food-with-copy-space.jpg'
    ]; // Replace with your actual image paths

    let currentIndex = 0;

    function preloadImage(src, callback) {
        const img = new Image();
        img.src = src;
        img.onload = callback;
    }

    function changeBackgroundImage() {
        section.classList.add('fade-out'); // Start fade-out effect

        // Preload the next image and set it after preload completes
        preloadImage(images[(currentIndex + 1) % images.length], () => {
            setTimeout(() => {
                currentIndex = (currentIndex + 1) % images.length;
                section.style.backgroundImage = `url('${images[currentIndex]}')`;
                section.classList.remove('fade-out'); // Remove fade-out
                section.classList.add('fade-in');    // Start fade-in effect
            }, 500); // Adjusted delay to ensure smoother transition
        });
    }

    setInterval(changeBackgroundImage, 6000); // Change image every 6 seconds
});


//// JavaScript for Menu Search and Filter
//document.addEventListener('DOMContentLoaded', function () {
//    const searchInput = document.getElementById('search');
//    const searchButton = document.getElementById('search-btn');
//    const menuItems = document.querySelectorAll('.menu-item');
//    const menuButtons = document.querySelectorAll('.menu-btn');

//    // Filter by Category
//    menuButtons.forEach(button => {
//        button.addEventListener('click', function () {
//            const category = this.getAttribute('data-category');

//            menuButtons.forEach(btn => btn.classList.remove('active'));
//            this.classList.add('active');

//            menuItems.forEach(item => {
//                if (category === 'all' || item.getAttribute('data-category') === category) {
//                    item.style.display = 'block';
//                } else {
//                    item.style.display = 'none';
//                }
//            });
//        });
//    });

//    // Search by Item Name or Cuisine Type
//    searchButton.addEventListener('click', function () {
//        const searchText = searchInput.value.toLowerCase();

//        menuItems.forEach(item => {
//            const name = item.getAttribute('data-name').toLowerCase();
//            const cuisine = item.getAttribute('data-cuisine').toLowerCase();
//            if (name.includes(searchText) || cuisine.includes(searchText)) {
//                item.style.display = 'block';
//            } else {
//                item.style.display = 'none';
//            }
//        });
//    });
//});


document.addEventListener("DOMContentLoaded", function () {
    // Function to simulate parking availability
    function updateParkingStatus() {
        // Simulated data (in a real scenario, this would come from an API or backend)
        const statuses = [
            { status: 'Available', spaces: 20, class: 'available' },
            { status: 'Limited', spaces: 5, class: 'available' },
            { status: 'Full', spaces: 0, class: 'full' }
        ];

        // Randomly select a status
        const currentStatus = statuses[Math.floor(Math.random() * statuses.length)];

        // Update the DOM
        const statusText = document.getElementById('status-text');
        const spacesLeft = document.getElementById('spaces-left');
        const parkingStatus = document.getElementById('parking-status');

        statusText.textContent = currentStatus.status;
        spacesLeft.textContent = currentStatus.spaces;
        parkingStatus.className = `status ${currentStatus.class}`;
    }

    // Update parking status every 10 seconds
    setInterval(updateParkingStatus, 10000);

    // Initial update
    updateParkingStatus();
});



