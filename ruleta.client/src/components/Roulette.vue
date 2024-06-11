<template>
    <div class="roulette-container">
        <div class="wheel-container">
            <canvas ref="canvas" width="400" height="400"></canvas>
            <div class="arrow"></div>
        </div>
        <button @click="launchWheel">Girar</button>
        <div class="user-info">
            <p>Usuario: {{ userName }}</p>
            <p>Saldo: {{ userBalance }}</p>
            <p>Apuesta: {{ amount }}</p>
        </div>
    </div>
</template>

<script>
    import { defineComponent } from 'vue';

    export default defineComponent({
        props: {
            userName: {
                type: String,
                required: true
            },
            userBalance: {
                type: Number,
                required: true
            },
            amount: {
                type: Number,
                required: true
            },
            betType: {
                type: String,
                required: true
            },
            betValue: {
                type: String,
                required: true
            },
            betNumber: {
                type: Number,
                required: true
            },
            betColor: {
                type: String,
                required: true
            }
        },
        data() {
            return {
                sectors: [
                    { value: '0', color: '#32CD32' },
                    { value: '1', color: '#8B0000' },
                    { value: '2', color: '#000000' },
                    { value: '3', color: '#8B0000' },
                    { value: '4', color: '#000000' },
                    { value: '5', color: '#8B0000' },
                    { value: '6', color: '#000000' },
                    { value: '7', color: '#8B0000' },
                    { value: '8', color: '#000000' },
                    { value: '9', color: '#8B0000' },
                    { value: '10', color: '#000000' },
                    { value: '11', color: '#8B0000' },
                    { value: '12', color: '#000000' },
                    { value: '13', color: '#8B0000' },
                    { value: '14', color: '#000000' },
                    { value: '15', color: '#8B0000' },
                    { value: '16', color: '#000000' },
                    { value: '17', color: '#8B0000' },
                    { value: '18', color: '#000000' },
                    { value: '19', color: '#8B0000' },
                    { value: '20', color: '#000000' },
                    { value: '21', color: '#8B0000' },
                    { value: '22', color: '#000000' },
                    { value: '23', color: '#8B0000' },
                    { value: '24', color: '#000000' },
                    { value: '25', color: '#8B0000' },
                    { value: '26', color: '#000000' },
                    { value: '27', color: '#8B0000' },
                    { value: '28', color: '#000000' },
                    { value: '29', color: '#8B0000' },
                    { value: '30', color: '#000000' },
                    { value: '31', color: '#8B0000' },
                    { value: '32', color: '#000000' },
                    { value: '33', color: '#8B0000' },
                    { value: '34', color: '#000000' },
                    { value: '35', color: '#8B0000' },
                    { value: '36', color: '#000000' }
                ],
                deg: 0
            };
        },
        mounted() {
            this.drawRouletteWheel();
        },
        methods: {
            drawRouletteWheel() {
                const canvas = this.$refs.canvas;
                const ctx = canvas.getContext('2d');
                const centerX = canvas.width / 2;
                const centerY = canvas.height / 2;
                const arc = Math.PI / (this.sectors.length / 2);

                ctx.clearRect(0, 0, canvas.width, canvas.height);

                this.sectors.forEach((sector, i) => {
                    const angle = i * arc;
                    ctx.beginPath();
                    ctx.fillStyle = sector.color;
                    ctx.moveTo(centerX, centerY);
                    ctx.arc(centerX, centerY, centerX, angle, angle + arc);
                    ctx.lineTo(centerX, centerY);
                    ctx.fill();
                    ctx.save();
                    ctx.translate(centerX, centerY);
                    ctx.rotate(angle + arc / 2);
                    ctx.textAlign = 'right';
                    ctx.fillStyle = '#ffffff';
                    ctx.font = 'bold 20px sans-serif';
                    ctx.fillText(sector.value, centerX - 10, 10);
                    ctx.restore();
                });
            },
            launchWheel() {
                this.deg = Math.floor(5000 + Math.random() * 5000);
                const canvas = this.$refs.canvas;

                canvas.style.transition = 'transform 5s ease-out';
                canvas.style.transform = `rotate(${this.deg}deg)`;

                setTimeout(() => {
                    canvas.style.transition = 'none';
                    const actualDeg = this.deg % 360;
                    canvas.style.transform = `rotate(${actualDeg}deg)`;

                    const arc = 360 / this.sectors.length;
                    const sectorIndex = Math.floor((360 - actualDeg) / arc) % this.sectors.length;
                    const winningSector = this.sectors[sectorIndex];

                    this.$emit('spinCompleted', winningSector.value, winningSector.color);
                }, 5000);
            }
        }
    });
</script>

<style scoped>
    .roulette-container {
        display: flex;
        flex-direction: column;
        align-items: center;
        background-color: #808080;
        padding: 20px;
        border-radius: 10px;
    }

    .wheel-container {
        position: relative;
        width: 400px;
        height: 400px;
    }

    .arrow {
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%) rotate(90deg);
        width: 0;
        height: 0;
        border-left: 20px solid transparent;
        border-right: 20px solid transparent;
        border-bottom: 30px solid #0094ff;
        z-index: 1;
    }

    button {
        margin-top: 20px;
        padding: 10px 20px;
        background-color: #007bff;
        color: white;
        border: none;
        border-radius: 5px;
        cursor: pointer;
    }

        button:hover {
            background-color: #0056b3;
        }

    .user-info {
        margin-top: 20px;
        color: white;
    }
</style>
