<script>
 import axios from "axios";
 import { goto } from "$app/navigation";

let username = "";
let password = "";
let email="";

async function register() {
  try {
    const response = await axios.post("https://localhost:7100/Auth/register", {
      username,
      email,
      password,
    });

    // Handle successful registration and navigate to the next page
  } catch (error) {
    // Handle registration error
  }
  await goto('/login');
}

</script>


<form on:submit|preventDefault={register}>
    <h1 class="h3 mb-3 fw-normal">Please register</h1>

    <input bind:value={username} class="form-control" placeholder="username" required>

    <input bind:value={email} type="email" class="form-control" placeholder="Email" required>

    <input bind:value={password} type="password" class="form-control" placeholder="Password" required>

    <button class="w-100 btn btn-lg btn-primary" type="submit">Submit</button>
</form>

<style>
  /* Styles go here */
</style>
