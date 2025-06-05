async function SessionExpired()
{
    await localStorageFunctions.removeItem("token");
    alert("Your session has expired. Please log in again.");
}

// wwwroot/localStorage.js
window.localStorageFunctions = {
    setItem: async function (key, value) {
        localStorage.setItem(key, value);
    },
    getItem: async function (key) {
        return localStorage.getItem(key);
    },
    removeItem: async function (key) {
        localStorage.removeItem(key);
    }
};