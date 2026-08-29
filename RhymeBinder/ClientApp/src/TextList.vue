<script setup>
    import { ref, reactive, computed } from 'vue'

    const props = defineProps({ initialData: Object })

    const texts = ref(props.initialData.textHeaders)
    const groupSequenceView = props.initialData.groupSequenceView
    const columns = props.initialData.columns

    // Not carrying over the legacy sort state here - it used header-label
    // strings like "Last Edited" that don't map 1:1 to property names.
    // Starting from a plain default instead; can revisit if you want continuity.
    const sortField = ref('title')
    const sortDescending = ref(false)
    const searchTerm = ref(props.initialData.searchValue || '')
    const selected = reactive({})

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
    <div class="menu-bar-item">
        <input type="text" v-model="searchTerm" placeholder="Search titles..." />
    </div>

    <table>
        <tr>
            <th></th>
            <th v-if="groupSequenceView" @click="setSort('groupSequence')">Sequence</th>
            <th v-else></th>
            <th @click="setSort('title')">Title</th>
            <th v-if="columns.lastModified" @click="setSort('lastModified')">Last Edited</th>
            <th v-if="columns.lastModifiedBy" @click="setSort('modifyByName')">Last Edited By</th>
            <th v-if="columns.created" @click="setSort('created')">Created</th>
            <th v-if="columns.createdBy" @click="setSort('createdByName')">Created By</th>
            <th v-if="columns.visionNumber" @click="setSort('visionNumber')">Vision Number</th>
            <th v-if="columns.revisionStatus" @click="setSort('revisionStatus')">Revision Status</th>
            <th v-if="columns.groups">Groups</th>
            <th v-if="columns.wordCount" @click="setSort('wordCount')">Word Count</th>
            <th v-if="columns.characterCount" @click="setSort('characterCount')">Character Count</th>
        </tr>
        <tr v-for="(text, index) in visibleTexts" :key="text.textHeaderId">
            <td>
                <input type="hidden" :name="`TextHeaders[${index}].TextHeaderId`" :value="text.textHeaderId" />
                <input type="checkbox" :name="`TextHeaders[${index}].Selected`" value="true" v-model="selected[text.textHeaderId]" />
            </td>
            <td v-if="groupSequenceView">{{ text.groupSequence }}</td>
            <td v-else></td>
            <td><a class="link-item" :href="`/RhymeBinder/ViewText?textHeaderID=${text.textHeaderId}`">{{ text.title }}</a></td>
            <td v-if="columns.lastModified">{{ text.lastModified }}</td>
            <td v-if="columns.lastModifiedBy">{{ text.modifyByName }}</td>
            <td v-if="columns.created">{{ text.created }}</td>
            <td v-if="columns.createdBy">{{ text.createdByName }}</td>
            <td v-if="columns.visionNumber">{{ text.visionNumber }}</td>
            <td v-if="columns.revisionStatus">{{ text.revisionStatus }}</td>
            <td v-if="columns.groups">
                <a v-for="g in text.groups" :key="g.savedViewId" class="link-item" :href="`/RhymeBinder/ListTexts?viewID=${g.savedViewId}`">{{ g.groupTitle }}</a>
            </td>
            <td v-if="columns.wordCount">{{ text.wordCount }}</td>
            <td v-if="columns.characterCount">{{ text.characterCount }}</td>
        </tr>
    </table>

    <div style="font-style: italic;">Showing {{ visibleTexts.length }} of {{ texts.length }} texts</div>
</template>