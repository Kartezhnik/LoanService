<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { useRouter } from 'vue-router'
  import type { Loan, LoanFilter } from '@/types/loan'

  const loans = ref<Loan[]>([])
  const total = ref(0)
  const page = ref(1)
  const size = ref(10)
  const loading = ref(false)

  // Полный набор фильтров согласно ТЗ
  const filter = ref<LoanFilter>({
    status: 'All',
    minAmount: undefined,
    maxAmount: undefined,
    minTerm: undefined,
    maxTerm: undefined
  })

  const router = useRouter()

  // Хелпер для определения статуса (строка в БД)
  const isPublished = (status: any) => {
    if (!status) return false
    return String(status).toLowerCase() === 'published'
  }

  // Загрузка данных с учетом пагинации и всех фильтров
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
      if (!res.ok) throw new Error(`Ошибка: ${res.status}`)

      const data = await res.json()
      loans.value = data.items ?? []
      total.value = data.totalCount ?? 0
    } catch (err) {
      console.error('Не удалось загрузить заявки:', err)
    } finally {
      loading.value = false
    }
  }

  // Смена статуса (PATCH /api/loans/{id}/toggle)
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

      // Обновляем данные в списке без перезагрузки всей страницы
      loan.status = nextStatus
      loan.modifiedAt = new Date().toISOString()
    } catch (err) {
      alert('Ошибка при смене статуса')
      loan.status = oldStatus
    }
  }

  // Сброс фильтров
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
        Создать заявку
      </el-button>
    </div>

    <el-card class="filter-card">
      <el-form :inline="true" :model="filter" size="default">
        <el-form-item label="Статус">
          <el-select v-model="filter.status" style="width: 140px" @change="loadLoans">
            <el-option label="Все" value="All" />
            <el-option label="Опубликована" value="Published" />
            <el-option label="Снята" value="Unpublished" />
          </el-select>
        </el-form-item>

        <el-form-item label="Сумма">
          <el-input-number v-model="filter.minAmount" placeholder="От" :controls="false" @change="loadLoans" />
          <span class="range-separator">-</span>
          <el-input-number v-model="filter.maxAmount" placeholder="До" :controls="false" @change="loadLoans" />
        </el-form-item>

        <el-form-item label="Срок">
          <el-input-number v-model="filter.minTerm" placeholder="От" :controls="false" @change="loadLoans" />
          <span class="range-separator">-</span>
          <el-input-number v-model="filter.maxTerm" placeholder="До" :controls="false" @change="loadLoans" />
        </el-form-item>

        <el-form-item>
          <el-button type="info" plain @click="resetFilters">Сбросить</el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-table :data="loans" v-loading="loading" border stripe style="width: 100%">
      <el-table-column prop="number" label="Номер" width="160" sortable />

      <el-table-column prop="amount" label="Сумма" width="120">
        <template #default="{ row }">
          {{ Number(row.amount || 0).toLocaleString() }}
        </template>
      </el-table-column>

      <el-table-column prop="termValue" label="Срок" width="90" />
      <el-table-column prop="interestValue" label="Ставка %" width="110" />

      <el-table-column label="Статус" width="150">
        <template #default="{ row }">
          <el-tag :type="isPublished(row.status) ? 'success' : 'info'" effect="dark">
            {{ isPublished(row.status) ? 'Опубликована' : 'Снята' }}
          </el-tag>
        </template>
      </el-table-column>

      <el-table-column label="Даты (Создано / Изменено)" width="220">
        <template #default="{ row }">
          <div class="date-cell">
            <span class="label">C:</span> {{ new Date(row.createdAt).toLocaleString() }}
          </div>
          <div class="date-cell">
            <span class="label">И:</span> {{ new Date(row.modifiedAt).toLocaleString() }}
          </div>
        </template>
      </el-table-column>

      <el-table-column label="Действие" min-width="160">
        <template #default="{ row }">
          <el-button :type="isPublished(row.status) ? 'danger' : 'success'"
                     size="small"
                     @click="toggleStatus(row)">
            {{ isPublished(row.status) ? 'Снять с публикации' : 'Опубликовать' }}
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
