import { createRouter, createWebHistory } from 'vue-router';
import Game from '../components/Game.vue';

const routes = [
    {
        path: '/',
        name: 'Game',
        component: Game,
    },
];

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes,
});

export default router;
