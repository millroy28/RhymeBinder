<script setup>
import { reactive, computed, onMounted, onUnmounted } from 'vue'

const props = defineProps({ groups: Array, selectedTextHeaderIds: Array })
const emit = defineEmits(['close', 'submitted'])

const pendingChanges = reactive({}) // groupId -> true/false once user overrides the initial state

function membershipState(group) {
  const memberSet = new Set(group.memberTextHeaderIds)
  const matchCount = props.selectedTextHeaderIds.filter(id => memberSet.has(id)).length
  if (matchCount === 0) return 'none'
  if (matchCount === props.selectedTextHeaderIds.length) return 'all'
  return 'some'
}

function isChecked(group) {
  if (pendingChanges[group.textGroupId] !== undefined) return pendingChanges[group.textGroupId]
  return membershipState(group) === 'all'
}

function isIndeterminate(group) {
  return pendingChanges[group.textGroupId] === undefined && membershipState(group) === 'some'
}

function toggle(group) {
  pendingChanges[group.textGroupId] = !isChecked(group)
}

const selectedCount = computed(() => props.selectedTextHeaderIds.length)

onMounted(() => { document.body.style.pointerEvents = 'none' })
onUnmounted(() => { document.body.style.pointerEvents = 'all' })

async function submit() {
  const groupChanges = {}
  for (const g of props.groups) {
    if (pendingChanges[g.textGroupId] !== undefined) groupChanges[g.textGroupId] = pendingChanges[g.textGroupId]
  }
  const res = await fetch('/RhymeBinder/UpdateGroupMembershipJson', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ textHeaderIds: props.selectedTextHeaderIds, groupChanges })
  })
  if (!res.ok) return // TODO: surface an error state
  const data = await res.json()
    emit('submitted', data.updatedGroupsByTextId)
}
</script>

<template>
  <div class="group-modal">
    <div class="group-modal-header">Add / Remove Groups:</div>
    <div class="group-modal-content">
      <div v-for="g in groups" :key="g.textGroupId" class="group-modal-content-item">
        <input type="checkbox" :disabled="g.locked" :checked="isChecked(g)" :indeterminate="isIndeterminate(g)" @change="toggle(g)" />
        <div>{{ g.groupTitle }}</div>
      </div>
    </div>
    <div class="group-modal-footer">
      <div class="group-modal-count">
        {{ selectedCount === 0 ? 'No records selected!' : `Apply changes to ${selectedCount} selected records:` }}
      </div>
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