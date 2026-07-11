const CACHE_NAME = 'gestao-v1';

// Ativa o Service Worker imediatamente
self.addEventListener('install', event => {
    self.skipWaiting();
});

self.addEventListener('activate', event => {
    event.waitUntil(clients.claim());
});

// Responde às requisições buscando direto na rede
self.addEventListener('fetch', event => {
    event.respondWith(fetch(event.request));
});