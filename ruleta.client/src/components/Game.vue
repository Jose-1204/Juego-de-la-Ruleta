<template>
    <div class="game-container">
        <div v-if="!gameStarted">
            <h2>Iniciar Juego</h2>
            <label for="userName">Nombre de Usuario:</label>
            <input v-model="userName" type="text" id="userName" required>

            <label for="initialBalance">Saldo Inicial:</label>
            <input v-model.number="initialBalance" type="number" id="initialBalance" required>

            <button class="primary-button" @click="startGame">Comenzar Juego</button>
        </div>

        <div v-else>
            <Roulette :userName="userName"
                      :userBalance="userBalance"
                      :amount="amount"
                      :betType="betType"
                      :betValue="betValue"
                      :betNumber="betNumber"
                      :betColor="betColor"
                      @spinCompleted="handleSpinCompleted" />

            <div class="bet-section">
                <h3>Realizar Apuesta</h3>
                <label for="betType">Tipo de Apuesta:</label>
                <select v-model="betType" id="betType">
                    <option value="Color">Color</option>
                    <option value="EvenOdd">Par/Impar</option>
                    <option value="Number">Número</option>
                    <option value="NumberColor">Número y Color</option>
                </select>

                <div v-if="betType === 'Color' || betType === 'NumberColor'">
                    <label for="betColor">Color:</label>
                    <select v-model="betColor" id="betColor">
                        <option value="Rojo">Rojo</option>
                        <option value="Negro">Negro</option>
                    </select>
                </div>

                <div v-if="betType === 'Number' || betType === 'NumberColor'">
                    <label for="betNumber">Número:</label>
                    <input v-model.number="betNumber" type="number" id="betNumber" min="0" max="36">
                </div>

                <div v-if="betType === 'EvenOdd'">
                    <label for="betValue">Par/Impar:</label>
                    <select v-model="betValue" id="betValue">
                        <option value="Par">Par</option>
                        <option value="Impar">Impar</option>
                    </select>
                </div>

                <label for="amount">Monto a Apostar:</label>
                <input v-model.number="amount" type="number" id="amount" min="1">

                <button class="primary-button" @click="placeBet">Apostar</button>
            </div>

            <div class="bet-results" v-if="spinResult">
                <h2>Resultado de la Ruleta</h2>
                <p>Número: {{ spinResult.number }}</p>
                <p>Color: {{ spinResult.color }}</p>
                <p>Par/Impar: {{ spinResult.evenOdd }}</p>
                <h3>{{ betOutcome }}</h3>
                <h3>Total Ganado: {{ totalWon }}</h3>
                <button class="secondary-button" @click="saveUser">Guardar Partida</button>
            </div>
        </div>
    </div>
</template>

<script>
    import { defineComponent } from 'vue';
    import Roulette from './Roulette.vue';
    import axios from 'axios';

    export default defineComponent({
        components: { Roulette },
        data() {
            return {
                userName: '',
                initialBalance: 0,
                userBalance: 0,
                amount: 0,
                betType: 'Color',
                betValue: '',
                betNumber: null,
                betColor: '',
                spinResult: null,
                betOutcome: '',
                totalWon: 0,
                gameStarted: false
            };
        },
        methods: {
            startGame() {
                if (this.userName && this.initialBalance > 0) {
                    this.userBalance = this.initialBalance;
                    this.gameStarted = true;
                } else {
                    alert('Por favor, ingrese un nombre de usuario y un saldo inicial válido.');
                }
            },
            placeBet() {
                if (this.amount <= 0 || this.amount > this.userBalance) {
                    alert('Monto de apuesta no válido.');
                    return;
                }
                if (this.betType === 'Number' && (this.betNumber < 0 || this.betNumber > 36)) {
                    alert('Número de apuesta no válido.');
                    return;
                }
                this.userBalance -= this.amount;
                this.$refs.roulette.launchWheel();
            },
            handleSpinCompleted(number, color) {
                const evenOdd = number % 2 === 0 ? 'Par' : 'Impar';
                this.spinResult = { number, color, evenOdd };
                this.evaluateBet();
            },
            evaluateBet() {
                let wonAmount = 0;
                if (this.betType === 'Color' && this.betColor === this.spinResult.color) {
                    wonAmount = this.amount * 2;
                    this.betOutcome = 'Ganaste! Tu apuesta de color fue correcta.';
                } else if (this.betType === 'EvenOdd' && this.betValue === this.spinResult.evenOdd) {
                    wonAmount = this.amount * 2;
                    this.betOutcome = 'Ganaste! Tu apuesta par/impar fue correcta.';
                } else if (this.betType === 'Number' && this.betNumber === this.spinResult.number) {
                    wonAmount = this.amount * 35;
                    this.betOutcome = 'Ganaste! Tu apuesta de número fue correcta.';
                } else if (this.betType === 'NumberColor' && this.betNumber === this.spinResult.number && this.betColor === this.spinResult.color) {
                    wonAmount = this.amount * 36;
                    this.betOutcome = 'Ganaste! Tu apuesta de número y color fue correcta.';
                } else {
                    this.betOutcome = 'Perdiste. Mejor suerte la próxima vez.';
                }
                this.userBalance += wonAmount;
                this.totalWon += wonAmount;
            },
            async saveUser() {
                try {
                    await axios.post('/api/users/save', {
                        userName: this.userName,
                        balance: this.userBalance
                    });
                    alert('Usuario guardado exitosamente.');
                } catch (error) {
                    console.error('Error saving user:', error);
                    alert('Error saving user: ' + error.response.data);
                }
            },
            async loadUser() {
                try {
                    const response = await axios.get(`/api/users/load?userName=${this.userName}`);
                    this.userBalance = response.data.balance;
                    alert('Usuario cargado exitosamente.');
                } catch (error) {
                    console.error('Error loading user:', error);
                    alert('Error loading user: ' + error.response.data);
                }
            }
        }
    });
</script>

<style scoped>
    .game-container {
        text-align: center;
        background-color: #f0f8ff;
        padding: 30px;
        border-radius: 10px;
        max-width: 600px;
        margin: 0 auto;
        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
    }

    button {
        margin-top: 20px;
        padding: 10px 20px;
        border: none;
        border-radius: 5px;
        cursor: pointer;
    }

    .primary-button {
        background-color: #007bff;
        color: white;
    }

        .primary-button:hover {
            background-color: #0056b3;
        }

    .secondary-button {
        background-color: #6c757d;
        color: white;
    }

        .secondary-button:hover {
            background-color: #5a6268;
        }

    .user-info {
        margin-top: 20px;
    }

    label {
        display: block;
        margin-top: 10px;
        font-weight: bold;
    }

    input, select {
        display: block;
        width: calc(100% - 20px);
        padding: 10px;
        margin-top: 5px;
        border-radius: 5px;
        border: 1px solid #ced4da;
    }

    .bet-results {
        margin-top: 20px;
        text-align: left;
        color: #212529;
    }

        .bet-results h2, .bet-results h3 {
            margin-bottom: 10px;
        }
</style>
