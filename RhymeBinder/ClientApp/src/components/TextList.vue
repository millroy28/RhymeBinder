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
    import { ref, reactive, computed } from 'vue'
    import { formatDate, formatNumber } from '../formatters.js'

    const props = defineProps({ initialData: Object })

    const texts = ref(props.initialData.textHeaders)
    const groupSequenceView = props.initialData.groupSequenceView
    const columns = reactive({ ...props.initialData.columns }) // now Vue-owned, ready to wire a toggle to later

    const sortField = ref('title')
    const sortDescending = ref(false)
    const searchTerm = ref(props.initialData.searchValue || '')
    const selected = reactive({})

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

    const filteredTexts = computed(() =>
        !searchTerm.value
            ? texts.value
            : texts.value.filter(t => t.title?.toLowerCase().includes(searchTerm.value.toLowerCase()))
    )

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
</script>

<template>
    <div class="menu-bar-secondary">
        <div class="menu-bar-title">Filters</div>
        <div class="menu-bar-item">
            <input type="text" v-model="searchTerm" placeholder="Title..." />
        </div>
        <div class="menu-bar-item">
            Groups:
        </div>
    </div>

    <div class="table-scroll-region">

        <table>
            <tr>
                <th></th>
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
</template>