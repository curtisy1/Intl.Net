// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.onreadystatechange = () => {
  if(document.readyState === "interactive" || document.readyState === "complete")  {
    document.getElementById("cultureSelect").onchange = setLanguage;
  }
}

async function setLanguage(e) {
  await fetch(`/Home/SetLanguage?language=${e.currentTarget.value}`);
}
