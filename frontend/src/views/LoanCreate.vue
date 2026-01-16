<script setup lang="ts">
  import { ref } from 'vue'
  import { useRouter } from 'vue-router'
  import type { LoanCreateDto } from '@/types/loan'

  const form = ref<LoanCreateDto>({
    number: '',
    amount: 0,
    termValue: 0,
    interestValue: 0
  })

  const rules = {
    amount: [{ required: true, message: 'Сумма > 0', trigger: 'blur' }],
    termValue: [{ required: true, message: 'Срок > 0', trigger: 'blur' }],
    interestValue: [{ required: true, message: 'Ставка > 0', trigger: 'blur' }]
  }

  const router = useRouter()

  async function submit() {

    if (form.value.amount <= 0) return alert('Сумма должна быть > 0')
    if (form.value.termValue <= 0) return alert('Срок > 0')
    if (form.value.interestValue <= 0) return alert('Ставка > 0')

    try {
      const res = await fetch('/api/loans', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(form.value)
      })
      if (!res.ok) throw new Error()
      alert('Заявка создана!')
      router.push('/loans')
    } catch {
      alert('Ошибка при создании')
    }
  }
</script>

<template>
  <el-form :model="form" :rules="rules" label-width="140px">
    <el-form-item label="Сумма" prop="amount">
      <el-input-number v-model="form.amount" :min="1" controls-position="right" />
    </el-form-item>
    <el-form-item label="Срок" prop="termValue">
      <el-input-number v-model="form.termValue" :min="1" controls-position="right" />
    </el-form-item>
    <el-form-item label="Процентная ставка" prop="interestValue">
      <el-input-number v-model="form.interestValue" :min="0.1" :precision="2" controls-position="right" />
    </el-form-item>
    <el-form-item>
      <el-button type="primary" @click="submit">Создать заявку</el-button>
    </el-form-item>
  </el-form>
</template>
