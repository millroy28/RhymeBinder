import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import TextList from './components/TextList.vue'


const dataEl = document.getElementById('list-texts-data')
const initialData = JSON.parse(dataEl.textContent)

createApp(TextList, { initialData }).mount('#list-texts-app')
