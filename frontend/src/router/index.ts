import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'

const routes: RouteRecordRaw[] = [                     
  { path: '/', redirect: '/loans' },
  {
    path: '/loans',
    name: 'LoanList',
    component: () => import('../views/LoanList.vue')
  },
  {
    path: '/loans/create',
    name: 'LoanCreate',
    component: () => import('../views/LoanCreate.vue')
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
