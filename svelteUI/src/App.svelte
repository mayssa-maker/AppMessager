<script>
  // Import necessary modules and components
  import { Router } from 'svelte-routing';
  import Navbar from "./Components/Navbar.svelte"
  import login  from "./Components/loginComponent.svelte";
  import Registration from './Components/Registration.svelte';
  

  // Define routes
  const routes = [
    {
      name: '/login',
      component: login,
    },
    {
      name: '/register',
      component: Registration,
    },
  ];
</script>

<main>
  <Navbar />

  <Router {routes} />
</main>

<style>
  /* Add any necessary global styles or component styles here */
</style>
