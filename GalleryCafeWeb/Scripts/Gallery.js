// Wait for the DOM to load
document.addEventListener('DOMContentLoaded', () => {
    const galleryItems = document.querySelectorAll('.gallery-item img');
    const lightbox = document.getElementById('lightbox');
    const lightboxImg = document.getElementById('lightbox-img');
    const caption = document.getElementById('caption');
    const closeBtn = document.querySelector('.close');

    // Function to open lightbox
    const openLightbox = (src, alt) => {
        lightbox.style.display = 'block';
        lightboxImg.src = src;
        caption.textContent = alt;
        document.body.style.overflow = 'hidden'; // Prevent background scrolling
    };

    // Function to close lightbox
    const closeLightbox = () => {
        lightbox.style.display = 'none';
        lightboxImg.src = '';
        caption.textContent = '';
        document.body.style.overflow = 'auto';
    };

    // Add click event to each gallery image
    galleryItems.forEach(img => {
        img.addEventListener('click', () => {
            openLightbox(img.src, img.alt);
        });
    });

    // Close lightbox when clicking on close button
    closeBtn.addEventListener('click', closeLightbox);

    // Close lightbox when clicking outside the image
    lightbox.addEventListener('click', (e) => {
        if (e.target === lightbox) {
            closeLightbox();
        }
    });

    // Close lightbox with 'Esc' key
    document.addEventListener('keydown', (e) => {
        if (e.key === 'Escape' && lightbox.style.display === 'block') {
            closeLightbox();
        }
    });
});