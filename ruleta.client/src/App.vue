<template>
    <div id="app">
        <header>
            <h1>Juego de la Ruleta</h1>
        </header>
        <main>
            <Game />
        </main>
    </div>
</template>

<script setup>
    import Game from './components/Game.vue';
</script>

<style scoped>
    #app {
        font-family: Avenir, Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;
        color: #2c3e50;
        margin-top: 60px;
    }

    header {
        background-color: #42b983;
        padding: 20px;
        color: white;
    }
</style>
