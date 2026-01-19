<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { useRouter } from 'vue-router'
  import type { Loan, LoanFilter } from '@/types/loan'

  const loans = ref<Loan[]>([])
  const total = ref(0)
  const page = ref(1)
  const size = ref(10)
  const loading = ref(false)

  const filter = ref<LoanFilter>({
    status: 'All',
    minAmount: undefined,
    maxAmount: undefined,
    minTerm: undefined,
    maxTerm: undefined
  })

  const router = useRouter()

  const isPublished = (status: any) => {
    if (!status) return false
    return String(status).toLowerCase() === 'published'
  }

  async function loadLoans() {
    loading.value = true
    try {
      const params = new URLSearchParams()
      params.append('pageNumber', page.value.toString())
      params.append('pageSize', size.value.toString())

      if (filter.value.status && filter.value.status !== 'All') {
        params.append('status', filter.value.status)
      }
      if (filter.value.minAmount !== undefined) params.append('minAmount', filter.value.minAmount.toString())
      if (filter.value.maxAmount !== undefined) params.append('maxAmount', filter.value.maxAmount.toString())
      if (filter.value.minTerm !== undefined) params.append('minTerm', filter.value.minTerm.toString())
      if (filter.value.maxTerm !== undefined) params.append('maxTerm', filter.value.maxTerm.toString())

      const res = await fetch(`/api/loans?${params.toString()}`)
      if (!res.ok) throw new Error(`РћС€РёР±РєР°: ${res.status}`)

      const data = await res.json()
      loans.value = data.items ?? []
      total.value = data.totalCount ?? 0
    } catch (err) {
      console.error('РќРµ СѓРґР°Р»РѕСЃСЊ Р·Р°РіСЂСѓР·РёС‚СЊ Р·Р°СЏРІРєРё:', err)
    } finally {
      loading.value = false
    }
  }

  async function toggleStatus(loan: Loan) {
    const oldStatus = loan.status
    const nextStatus = isPublished(loan.status) ? 'Unpublished' : 'Published'

    try {
      const res = await fetch(`/api/loans/${loan.id}/toggle`, {
        method: 'PATCH',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ id: loan.id })
      })

      if (!res.ok) throw new Error()

      loan.status = nextStatus
      loan.modifiedAt = new Date().toISOString()
    } catch (err) {
      alert('РћС€РёР±РєР° РїСЂРё СЃРјРµРЅРµ СЃС‚Р°С‚СѓСЃР°')
      loan.status = oldStatus
    }
  }

  // РЎР±СЂРѕСЃ С„РёР»СЊС‚СЂРѕРІ
  function resetFilters() {
    filter.value = {
      status: 'All',
      minAmount: undefined,
      maxAmount: undefined,
      minTerm: undefined,
      maxTerm: undefined
    }
    loadLoans()
  }

  onMounted(loadLoans)
</script>

<template>
  <div class="loans-page">
    <div class="header-actions">
      <el-button type="primary" @click="router.push('/loans/create')">
        РЎРѕР·РґР°С‚СЊ Р·Р°СЏРІРєСѓ
      </el-button>
    </div>

    <el-card class="filter-card">
      <el-form :inline="true" :model="filter" size="default">
        <el-form-item label="РЎС‚Р°С‚СѓСЃ">
          <el-select v-model="filter.status" style="width: 140px" @change="loadLoans">
            <el-option label="Р’СЃРµ" value="All" />
            <el-option label="РћРїСѓР±Р»РёРєРѕРІР°РЅР°" value="Published" />
            <el-option label="РЎРЅСЏС‚Р°" value="Unpublished" />
          </el-select>
        </el-form-item>

        <el-form-item label="РЎСѓРјРјР°">
          <el-input-number v-model="filter.minAmount" placeholder="РћС‚" :controls="false" @change="loadLoans" />
          <span class="range-separator">-</span>
          <el-input-number v-model="filter.maxAmount" placeholder="Р”Рѕ" :controls="false" @change="loadLoans" />
        </el-form-item>

        <el-form-item label="РЎСЂРѕРє">
          <el-input-number v-model="filter.minTerm" placeholder="РћС‚" :controls="false" @change="loadLoans" />
          <span class="range-separator">-</span>
          <el-input-number v-model="filter.maxTerm" placeholder="Р”Рѕ" :controls="false" @change="loadLoans" />
        </el-form-item>

        <el-form-item>
          <el-button type="info" plain @click="resetFilters">РЎР±СЂРѕСЃРёС‚СЊ</el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-table :data="loans" v-loading="loading" border stripe style="width: 100%">
      <el-table-column prop="number" label="РќРѕРјРµСЂ" width="160" sortable />

      <el-table-column prop="amount" label="РЎСѓРјРјР°" width="120">
        <template #default="{ row }">
          {{ Number(row.amount || 0).toLocaleString() }}
        </template>
      </el-table-column>

      <el-table-column prop="termValue" label="РЎСЂРѕРє" width="90" />
      <el-table-column prop="interestValue" label="РЎС‚Р°РІРєР° %" width="110" />

      <el-table-column label="РЎС‚Р°С‚СѓСЃ" width="150">
        <template #default="{ row }">
          <el-tag :type="isPublished(row.status) ? 'success' : 'info'" effect="dark">
            {{ isPublished(row.status) ? 'РћРїСѓР±Р»РёРєРѕРІР°РЅР°' : 'РЎРЅСЏС‚Р°' }}
          </el-tag>
        </template>
      </el-table-column>

      <el-table-column label="Р”Р°С‚С‹ (РЎРѕР·РґР°РЅРѕ / РР·РјРµРЅРµРЅРѕ)" width="220">
        <template #default="{ row }">
          <div class="date-cell">
            <span class="label">C:</span> {{ new Date(row.createdAt).toLocaleString() }}
          </div>
          <div class="date-cell">
            <span class="label">Р:</span> {{ new Date(row.modifiedAt).toLocaleString() }}
          </div>
        </template>
      </el-table-column>

      <el-table-column label="Р”РµР№СЃС‚РІРёРµ" min-width="160">
        <template #default="{ row }">
          <el-button :type="isPublished(row.status) ? 'danger' : 'success'"
                     size="small"
                     @click="toggleStatus(row)">
            {{ isPublished(row.status) ? 'РЎРЅСЏС‚СЊ СЃ РїСѓР±Р»РёРєР°С†РёРё' : 'РћРїСѓР±Р»РёРєРѕРІР°С‚СЊ' }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>

    <div class="pagination-container">
      <el-pagination v-model:current-page="page"
                     v-model:page-size="size"
                     :page-sizes="[10, 20, 50, 100]"
                     layout="total, sizes, prev, pager, next"
                     :total="total"
                     @current-change="loadLoans"
                     @size-change="(val) => { size = val; loadLoans(); }" />
    </div>
  </div>
</template>

<style scoped>
  .loans-page {
    padding: 20px;
  }

  .header-actions {
    margin-bottom: 20px;
  }

  .filter-card {
    margin-bottom: 20px;
    background-color: #f9fafc;
  }

  .range-separator {
    margin: 0 8px;
    color: #909399;
  }

  .date-cell {
    font-size: 12px;
    line-height: 1.4;
  }

    .date-cell .label {
      font-weight: bold;
      color: #606266;
    }

  .pagination-container {
    margin-top: 20px;
    display: flex;
    justify-content: flex-end;
  }
</style>
