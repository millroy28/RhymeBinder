<script setup>import { ref, onMounted, onUnmounted } from 'vue'

const props = defineProps({ binders: Array, selectedTextHeaderIds: Array })
const emit = defineEmits(['close', 'transferred'])
const destinationBinderId = ref(null)

onMounted(() => { document.body.style.pointerEvents = 'none' })
onUnmounted(() => { document.body.style.pointerEvents = 'all' })

async function submit() {
  if (!destinationBinderId.value) return
  const res = await fetch('/RhymeBinder/TransferTextsToBinderJson', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ textHeaderIds: props.selectedTextHeaderIds, destinationBinderId: destinationBinderId.value })
  })
  if (!res.ok) return
  emit('transferred', props.selectedTextHeaderIds)
}</script>

<template>
    <div class="group-modal">
        <div class="group-modal-header">Transfer to Binder:</div>
        <div class="group-modal-content" style="grid-template-columns: 1fr;">
            <div v-for="b in binders" :key="b.binderId" class="group-modal-content-item">
                <input type="radio" :id="`binder-${b.binderId}`" name="destinationBinder" :value="b.binderId" v-model="destinationBinderId" />
                <label :for="`binder-${b.binderId}`">{{ b.name }}</label>
            </div>
        </div>
        <div class="group-modal-footer">
            <div class="group-modal-count">{{ selectedTextHeaderIds.length }} record(s) selected</div>
            <a class="button menu-bar-button" @click="$emit('close')">Cancel</a>
            <a class="button menu-bar-button" @click="submit">Submit</a>
        </div>
    </div>
</template>

<style scoped>
.group-modal { position: absolute; z-index: 1; left: 20%; top: 10%; width: 60%; overflow: auto; box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2), 0 6px 20px 0 rgba(0,0,0,0.19); border: 2px solid; background-color: var(--color-bg-body); }
.group-modal-header { padding: 2px 16px; background-color: var(--color-bg-accent); font-size: larger; font-weight: 700; }
.group-modal-content { padding: 15px 20px; display: grid; grid-template-columns: repeat(3, auto); pointer-events: all; }
.group-modal-content-item { display: inline-flex; align-items: center; gap: 6px; padding-top: 5px; }
.group-modal-footer { width: 100%; background-color: var(--color-bg-accent); display: inline-flex; justify-content: flex-end; align-items: baseline; pointer-events: all; }
.group-modal-count { padding-right: 10px; font-weight: 600; }
</style>