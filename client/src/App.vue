<template>
  <div class="container">
    <div class="card">
      <h1 class="title">Знайти десяткові числа</h1>
      <input
        type="text"
        v-model="inputData"
        placeholder="Введіть дані..."
        class="input"
      />
      <button @click="sendRequest" class="button">Надіслати запит</button>
      <transition name="fade">
        <div v-if="responseMessage" class="response">
          <strong>{{ responseMessage }}</strong>
        </div>
      </transition>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "App",
  data() {
    return {
      inputData: "",
      responseMessage: null,
    };
  },
  methods: {
    async sendRequest() {
      try {
        const response = await axios.post(
          "http://10.0.2.15:3333/findDecimalNumbers",
          { data: this.inputData },
          {
            headers: { "Content-Type": "application/json" },
          }
        );
        
        // Перевіряємо, чи є знайдені числа
        if (response.data.decimalNumbers.length > 0) {
          this.responseMessage = `Знайдені числа: ${response.data.decimalNumbers.join(", ")}`;
        } else {
          this.responseMessage = "Десяткових чисел не знайдено";
        }
      } catch (error) {
        console.error("Помилка при відправленні запиту:", error);
        this.responseMessage = "Помилка під час отримання даних";
      }
    },
  },
};
</script>

<style>
/* General Styling */
.container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background: linear-gradient(135deg, #6e8efb, #a777e3);
}

.card {
  background: white;
  padding: 20px;
  border-radius: 12px;
  box-shadow: 0px 10px 30px rgba(0, 0, 0, 0.2);
  text-align: center;
  width: 350px;
  animation: fadeIn 0.5s ease-in-out;
}

.title {
  font-size: 22px;
  font-weight: bold;
  margin-bottom: 15px;
  color: #333;
}

/* Input Styling */
.input {
  width: 90%;
  padding: 10px;
  font-size: 16px;
  border: 2px solid #ccc;
  border-radius: 6px;
  outline: none;
  transition: 0.3s;
}

.input:focus {
  border-color: #6e8efb;
  box-shadow: 0 0 8px rgba(110, 142, 251, 0.6);
}

/* Button Styling */
.button {
  margin-top: 15px;
  padding: 10px 15px;
  font-size: 16px;
  font-weight: bold;
  background: #6e8efb;
  border: none;
  color: white;
  border-radius: 6px;
  cursor: pointer;
  transition: all 0.3s ease;
}

.button:hover {
  background: #5a7ae0;
  transform: scale(1.05);
}

.button:active {
  transform: scale(0.95);
}

/* Response Styling */
.response {
  margin-top: 15px;
  background: #e3f2fd;
  padding: 10px;
  border-radius: 6px;
  font-size: 16px;
  color: #333;
}

/* Animations */
@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(-20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.fade-enter-active, .fade-leave-active {
  transition: opacity 0.5s ease;
}
.fade-enter, .fade-leave-to {
  opacity: 0;
}
</style>
