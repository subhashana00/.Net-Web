document.querySelector('.login-form').addEventListener('submit', function (event) {
    // Basic form validation
    var username = document.getElementById('username').value;
    var password = document.getElementById('password').value;

    if (username === '' || password === '') {
        alert('Please fill out both fields.');
        event.preventDefault();
    }
});