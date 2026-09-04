<style scoped>

    .table-scroll-region {
        flex: 1 1 auto;
        min-height: 0;
        overflow-y: auto;
        font-size: small;
    }

        .table-scroll-region table {
            width: 100%;
            border-collapse: collapse;
            background: var(--color-bg-panel);
        }

        .table-scroll-region thead th {
            position: sticky;
            top: 0;
            background: var(--color-bg-panel);
            border-bottom-style: double;
            border-bottom-color: var(--color-table-rule);
            z-index: 1;
        }

    .list-texts-status-bar {
        flex: 0 0 auto;
        font-size: small;
        padding: 6px 12px;
        border-top: 1px solid var(--color-bg-hover);
        font-family: var(--font-ui);
    }
</style>

<script setup>
    import { ref, reactive, computed, onMounted, watch } from 'vue'
    import { formatDate, formatNumber } from '../formatters.js'
    import GroupAssignmentModal from './GroupAssignmentModal.vue'
    import BinderTransferModal from './BinderTransferModal.vue'

    const props = defineProps({ initialData: Object })

    const texts = ref(props.initialData.textHeaders)
    const groups = ref(props.initialData.groups)
    const groupSequenceView = props.initialData.groupSequenceView
    const columns = reactive({ ...props.initialData.columns }) // now Vue-owned, ready to wire a toggle to later

    const sortField = ref('title')
    const sortDescending = ref(false)
    const searchTerm = ref(props.initialData.searchValue || '')
    const selected = reactive({})

    const selectedIds = computed(() => Object.keys(selected).filter(id => selected[id]).map(Number))
    const showGroupModal = ref(false)
    const showBinderModal = ref(false)
    const selectedGroupIds = ref([])

    // Single source of truth for every "plain scalar" column: label, alignment,
    // formatting, sortability, and visibility all live here instead of being
    // scattered across the template. Title/Sequence/Groups are excluded because
    // they need custom markup (links, lists), not just a formatted value.
    const columnDefs = computed(() => [
        { key: 'lastModified', label: 'Last Edited', align: 'center', format: formatDate, sortable: true, visible: columns.lastModified },
        { key: 'modifyByName', label: 'Last Edited By', align: 'center', sortable: true, visible: columns.lastModifiedBy },
        { key: 'created', label: 'Created', align: 'center', format: formatDate, sortable: true, visible: columns.created },
        { key: 'createdByName', label: 'Created By', align: 'center', sortable: true, visible: columns.createdBy },
        { key: 'visionNumber', label: 'Vision Number', align: 'center', sortable: true, visible: columns.visionNumber },
        { key: 'revisionStatus', label: 'Revision Status', align: 'center', sortable: true, visible: columns.revisionStatus },
        { key: 'wordCount', label: 'Word Count', align: 'right', format: formatNumber, sortable: true, visible: columns.wordCount },
        { key: 'characterCount', label: 'Character Count', align: 'right', format: formatNumber, sortable: true, visible: columns.characterCount },
    ])

    const visibleColumnDefs = computed(() => columnDefs.value.filter(c => c.visible))

    function setSort(field) {
        if (sortField.value === field) {
            sortDescending.value = !sortDescending.value
        } else {
            sortField.value = field
            sortDescending.value = false
        }
    }

    const filteredTexts = computed(() => {
        let result = texts.value
        console.log("filterin!")
        if (searchTerm.value) {
            console.log("text filterin");
            result = result.filter(t => t.title?.toLowerCase().includes(searchTerm.value.toLowerCase()))
        }

        if (selectedGroupIds.length > 0) {
            console.log("group filterin");
            result = result.filter(t =>
                t.groups?.some(g => selectedGroupIds.value.includes(g.savedViewId))
            )
        }

        return result
    })

    const visibleTexts = computed(() => {
        const sorted = [...filteredTexts.value]
        sorted.sort((a, b) => {
            const av = a[sortField.value]
            const bv = b[sortField.value]
            if (av === bv) return 0
            const result = av > bv ? 1 : -1
            return sortDescending.value ? -result : result
        })
        return sorted
    })

    const allVisibleSelected = computed(() =>
        visibleTexts.value.length > 0 &&
        visibleTexts.value.every(t => selected[t.textHeaderId])
    )

    const someVisibleSelected = computed(() =>
        visibleTexts.value.some(t => selected[t.textHeaderId]) && !allVisibleSelected.value
    )

    function toggleSelectAll(event) {
        const checked = event.target.checked
        for (const t of visibleTexts.value) {
            selected[t.textHeaderId] = checked
        }
    }

    const selectAllCheckbox = ref(null)
    watch(someVisibleSelected, (val) => {
        if (selectAllCheckbox.value) selectAllCheckbox.value.indeterminate = val
    })

    function clearFilters() {
        searchTerm.value = ''
        selectedGroupIds.value = []
    }

    function handleGroupsSubmitted(updatedGroupsByTextId) {
        // Table's Groups column - already working, unchanged
        for (const t of texts.value) {
            if (updatedGroupsByTextId[t.textHeaderId]) t.groups = updatedGroupsByTextId[t.textHeaderId]
        }

        // Modal's own membership data - the actual fix
        const affectedTextIds = Object.keys(updatedGroupsByTextId).map(Number)
        for (const group of groups.value) {
            // Drop every affected text from this group's membership list, then
            // re-add only the ones the server confirms are still (or newly) members
            group.memberTextHeaderIds = group.memberTextHeaderIds.filter(id => !affectedTextIds.includes(id))
            for (const textId of affectedTextIds) {
                const stillMember = updatedGroupsByTextId[textId].some(g => g.textGroupId === group.textGroupId)
                if (stillMember) group.memberTextHeaderIds.push(textId)
            }
        }

        showGroupModal.value = false
    }

    function handleTransferred(transferredIds) {
        const idSet = new Set(transferredIds)
        texts.value = texts.value.filter(t => !idSet.has(t.textHeaderId))
        transferredIds.forEach(id => delete selected[id])
        showBinderModal.value = false
    }

    onMounted(() => {
        window.listTextsActions = {
            openGroupModal: () => { showGroupModal.value = true },
            openBinderModal: () => { showBinderModal.value = true }
        }
    })
</script>

<template>
    <!--Filters-->
    <div class="menu-bar-secondary">
        <div class="menu-bar-title">Filters</div>
        <div class="menu-bar-item">
            <input type="text" v-model="searchTerm" placeholder="Title..." />
        </div>
        <div class="menu-bar-item">
            <label>Groups:</label>
            <select v-model="selectedGroupIds" size="1">
                <option v-for="g in groups" :key="g.groupTitle" :value="g.groupTitle">{{ g.groupTitle }}</option>
            </select>
        </div>
        <div class="menu-bar-item">
            <button type="button" @click="clearFilters">Clear filters</button>
        </div>
    </div>

    <!--Table-->
    <div class="table-scroll-region">

        <table>
            <tr>
                <th>
                    <input type="checkbox"
                           ref="selectAllCheckbox"
                           :checked="allVisibleSelected"
                           @change="toggleSelectAll" />
                </th>
                <th v-if="groupSequenceView" @click="setSort('groupSequence')">Sequence</th>
                <th v-else></th>
                <th @click="setSort('title')">Title</th>
                <th v-for="col in visibleColumnDefs"
                    :key="col.key"
                    :class="`align-${col.align}`"
                    @click="col.sortable && setSort(col.key)">{{ col.label }}</th>
                <th v-if="columns.groups">Groups</th>
            </tr>

            <tr v-for="(text, index) in visibleTexts" :key="text.textHeaderId">
                <td>
                    <input type="hidden" :name="`TextHeaders[${index}].TextHeaderId`" :value="text.textHeaderId" />
                    <input type="checkbox" :name="`TextHeaders[${index}].Selected`" value="true" v-model="selected[text.textHeaderId]" />
                </td>
                <td v-if="groupSequenceView">{{ text.groupSequence }}</td>
                <td v-else></td>
                <td><a class="link-item" :href="`/RhymeBinder/ViewText?textHeaderID=${text.textHeaderId}`">{{ text.title }}</a></td>
                <td v-for="col in visibleColumnDefs" :key="col.key" :class="`align-${col.align}`">
                    {{ col.format ? col.format(text[col.key]) : text[col.key] }}
                </td>
                <td v-if="columns.groups">
                    <template v-for="(g, i) in text.groups" :key="g.savedViewId">
                        <a class="link-item" :href="`/RhymeBinder/ListTexts?viewID=${g.savedViewId}`">{{ g.groupTitle }}</a>

                    </template>
                </td>
            </tr>
        </table>
    </div>

    <div style="font-style: italic;">Showing {{ visibleTexts.length }} of {{ texts.length }} texts</div>

    <GroupAssignmentModal v-if="showGroupModal"
                          :groups="groups"
                          :selectedTextHeaderIds="selectedIds"
                          @close="showGroupModal = false"
                          @submitted="handleGroupsSubmitted" />
    <BinderTransferModal v-if="showBinderModal"
                         :binders="props.initialData.userBinders"
                         :selectedTextHeaderIds="selectedIds"
                         @close="showBinderModal = false"
                         @transferred="handleTransferred" />
</template>