<script>
  import { onMount } from "svelte";
  import { goto } from "$app/navigation";
  import { authenticated } from '../stores/auth.js';
  import LoginComponent from "../routes/LoginComponent.svelte";
  import RegisterComponent from "../routes/RegisterComponent.svelte";

    let auth = false;

  const unsubscribe = authenticated.subscribe(value => {
    auth = value;
  });

  onMount(() => {
    unsubscribe();
  });
   function navigateToLogin() {
// Render the RegisterComponent
const component = new LoginComponent({
target: document.body,
});

}

function navigateToRegister() {
// Render the RegisterComponent
const component = new RegisterComponent({
target: document.body,
});

}

  

</script>

<nav class="navbar navbar-expand-md navbar-dark bg-dark mb-4">
<button on:click={navigateToLogin}>Go to Login</button>
<button on:click={navigateToRegister}>Go to Register</button>

 
</nav>



