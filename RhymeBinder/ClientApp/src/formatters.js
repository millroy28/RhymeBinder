export function formatDate(value) {
    if (!value) return ''
    const d = new Date(value)
    if (isNaN(d)) return value
    return d.toLocaleDateString(undefined, { year: 'numeric', month: 'short', day: 'numeric' })
}

export function formatNumber(value) {
    if (value == null) return ''
    return Number(value).toLocaleString()
}