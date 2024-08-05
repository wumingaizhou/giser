<template>
  <div id="app">
    <el-container>
      <el-header>一张图地图</el-header>
      <el-container>
        <el-aside width="200px">Aside</el-aside>
        <el-container class="mapContainer">
          <el-main>
            <Mapview />
          </el-main>
        </el-container>
      </el-container>
    </el-container>
  </div>
</template>

<script>
import Mapview from './components/common/Mapview.vue';
export default {
  name: 'App',
  components: {
    Mapview,
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

.el-header {
  background-color: #909399;
  color: #333;
  text-align: center;
  line-height: 60px;
  border-radius: 5px;
}

.el-aside {
  background-color: #D3DCE6;
  color: #333;
  text-align: center;
  line-height: 200px;
  height: 100vh;
  border-radius: 5px;
}

.mapContainer {
  height: 100vh;
}

.el-main {
  background-color: #E9EEF3;
  color: #333;
  text-align: center;
  line-height: 160px;
  height: 100%;
}
</style>
