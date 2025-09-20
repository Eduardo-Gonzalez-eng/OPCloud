// Import the functions you need from the SDKs you need
import { initializeApp } from "https://www.gstatic.com/firebasejs/12.3.0/firebase-app.js";
import {
    getAuth,
    createUserWithEmailAndPassword,
    signInWithEmailAndPassword,
    signOut,
    onAuthStateChanged,
    updateProfile
} from "https://www.gstatic.com/firebasejs/12.3.0/firebase-auth.js";

const firebaseConfig = {
    apiKey: "AIzaSyBgIuBw31VRnwaeSeKRLdYVgOVTKOqI2Mw",
    authDomain: "opcloud-d40bc.firebaseapp.com",
    projectId: "opcloud-d40bc",
    storageBucket: "opcloud-d40bc.firebasestorage.app",
    messagingSenderId: "1094694845322",
    appId: "1:1094694845322:web:d278c79383dfcf994d2673"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);
const auth = getAuth(app);

// Firebase Auth Functions
export async function createUser(email, password, displayName = null) {
    try {
        const userCredential = await createUserWithEmailAndPassword(auth, email, password);

        // Si se pasa un displayName, lo actualizamos
        if (displayName) {
            await updateProfile(userCredential.user, { displayName });
        }

        return {
            success: true,
            user: {
                uid: userCredential.user.uid,
                email: userCredential.user.email,
                displayName: userCredential.user.displayName
            }
        };
    } catch (error) {
        return {
            success: false,
            error: {
                code: error.code,
                message: error.message,
                details: getFirebaseErrorDetails(error.code)
            }
        };
    }
}

export async function updateDisplayName(newName) {
    try {
        if (!auth.currentUser) {
            return {
                success: false,
                error: {
                    code: "no-user",
                    message: "No hay usuario autenticado"
                }
            };
        }

        await updateProfile(auth.currentUser, { displayName: newName });

        return {
            success: true,
            user: {
                uid: auth.currentUser.uid,
                email: auth.currentUser.email,
                displayName: auth.currentUser.displayName
            }
        };
    } catch (error) {
        return {
            success: false,
            error: {
                code: error.code || "unknown",
                message: error.message || "Error desconocido"
            }
        };
    }
}


export async function signIn(email, password) {
    try {
        const userCredential = await signInWithEmailAndPassword(auth, email, password);
        return {
            success: true,
            user: {
                uid: userCredential.user.uid,
                email: userCredential.user.email
            }
        };
    } catch (error) {
        return {
            success: false,
            error: {
                code: error.code,
                message: error.message,
                details: getFirebaseErrorDetails(error.code)
            }
        };
    }
}

export async function logout() {
    try {
        await signOut(auth);
        return { success: true };
    } catch (error) {
        return {
            success: false,
            error: {
                code: error.code,
                message: error.message
            }
        };
    }
}

export function getCurrentUser() {
    const user = auth.currentUser;
    return user ? {
        uid: user.uid,
        email: user.email,
        displayName: user.displayName,
        emailVerified: user.emailVerified
    } : null;
}

export function onAuthStateChange(callback) {
    return onAuthStateChanged(auth, (user) => {
        callback(user ? {
            uid: user.uid,
            email: user.email,
            displayName: user.displayName,
            emailVerified: user.emailVerified
        } : null);
    });
}

// Utility Functions
export function getCurrentTime() {
    alert("OJK");
    return new Date().toLocaleTimeString('es-ES', {
        hour: '2-digit',
        minute: '2-digit',
        second: '2-digit'
    });
}

export function getCurrentDate() {
    alert("SHOW");
    return new Date().toLocaleDateString('es-ES', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
    });
}

export function showNotification(message, type = 'info') {
    const notification = document.createElement('div');
    notification.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        padding: 15px 20px;
        border-radius: 5px;
        color: white;
        z-index: 10000;
        font-family: Arial, sans-serif;
        max-width: 300px;
        transition: opacity 0.3s ease;
    `;

    const colors = {
        success: '#52c41a',
        error: '#f5222d',
        warning: '#faad14',
        info: '#1890ff'
    };

    notification.style.backgroundColor = colors[type] || colors.info;
    notification.textContent = message;

    document.body.appendChild(notification);

    setTimeout(() => {
        notification.style.opacity = '0';
        setTimeout(() => notification.remove(), 300);
    }, 3000);
}

// Error handling
function getFirebaseErrorDetails(errorCode) {
    const errorMap = {
        'auth/email-already-in-use': 'El correo electrónico ya está en uso',
        'auth/invalid-email': 'El correo electrónico no es válido',
        'auth/operation-not-allowed': 'La operación no está permitida',
        'auth/weak-password': 'La contraseña es demasiado débil',
        'auth/user-disabled': 'El usuario ha sido deshabilitado',
        'auth/user-not-found': 'Usuario no encontrado',
        'auth/wrong-password': 'Contraseña incorrecta',
        'auth/too-many-requests': 'Demasiados intentos. Intenta más tarde'
    };

    return errorMap[errorCode] || 'Error desconocido';
}

// Storage utilities
export function setLocalStorage(key, value) {
    try {
        localStorage.setItem(key, JSON.stringify(value));
        return true;
    } catch (error) {
        console.error('Error saving to localStorage:', error);
        return false;
    }
}

export function getLocalStorage(key) {
    try {
        const item = localStorage.getItem(key);
        return item ? JSON.parse(item) : null;
    } catch (error) {
        console.error('Error reading from localStorage:', error);
        return null;
    }
}

export function removeLocalStorage(key) {
    try {
        localStorage.removeItem(key);
        return true;
    } catch (error) {
        console.error('Error removing from localStorage:', error);
        return false;
    }
}