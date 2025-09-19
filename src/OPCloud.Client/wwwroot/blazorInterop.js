// Importa las funciones del módulo principal
import {
    createUser,
    signIn,
    logout,
    getCurrentUser,
    onAuthStateChange,
    getCurrentTime,
    getCurrentDate,
    showNotification,
    setLocalStorage,
    getLocalStorage,
    removeLocalStorage
} from './js/app.js';

// Exponer funciones globalmente para Blazor
window.FirebaseInterop = {
    // Auth functions
    createUser: async (email, password) => {
        return await createUser(email, password);
    },

    signIn: async (email, password) => {
        return await signIn(email, password);
    },

    logout: async () => {
        return await logout();
    },

    getCurrentUser: () => {
        return getCurrentUser();
    },

    onAuthStateChange: (dotNetHelper) => {
        return onAuthStateChange((user) => {
            dotNetHelper.invokeMethodAsync('OnAuthStateChanged', user);
        });
    },

    // Utility functions
    getCurrentTime: () => {
        return getCurrentTime();
    },

    getCurrentDate: () => {
        return getCurrentDate();
    },

    showNotification: (message, type = 'info') => {
        showNotification(message, type);
    },

    // Storage functions
    setLocalStorage: (key, value) => {
        return setLocalStorage(key, value);
    },

    getLocalStorage: (key) => {
        return getLocalStorage(key);
    },

    removeLocalStorage: (key) => {
        return removeLocalStorage(key);
    },

    // Health check
    isInitialized: () => {
        return true;
    }
};

// Función de inicialización para verificar que todo esté cargado
window.initializeBlazorInterop = () => {
    console.log('✅ Blazor Interop initialized successfully');
    return true;
};

// Exponer funciones individuales para compatibilidad (opcional)
window.getCurrentTime = getCurrentTime;
window.showNotification = showNotification;

console.log("🚀 Blazor Interop script loaded successfully");