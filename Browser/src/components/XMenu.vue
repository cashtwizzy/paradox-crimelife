<template>
  <div id="xmenu" class="window" v-if="visible">
    <div id="XCircleMenu" class="circleMenu" v-show="visible">
      <div>
        <div
          v-for="item in items"
          :style="{
            top: item.y + 'px',
            left: item.x + 'px',
            height: itemRadius + 'px',
            width: itemRadius + 'px',
            'border-width': itemBorderWidth + 'px',
          }"
          :class="{ active: item === activeItem }"
          :key="item.id"
        >
          <img :src="require(`../assets/img/XMenu/${item.icon}.png`)" />
        </div>
      </div>

      <div
        id="circleMenuLabel"
        :style="{
          top: middlePoint.y - radius / 6.4 - 135 + 'px',
          left: middlePoint.x - radius * 0.55 + 'px',
          width: radius * 1.1 + 'px',
          'font-size': radius / 11.5 + 'px',
        }"
      >
        {{ activeItem.label }}
      </div>

      <div
        id="circleMenuDescription"
        :style="{
          top: middlePoint.y - radius / 6.4 - 97 + 'px',
          left: middlePoint.x - radius * 0.55 + 'px',
          width: radius * 1.1 + 'px',
          'font-size': radius / 15 + 'px',
        }"
      >
        {{ activeItem.description }}
      </div>
    </div>
  </div>
</template>

<script>
import * as d3 from "d3";

export default {
  name: "XMenu",
  data() {
    let itemBorderWidth = window.innerHeight * 0.004;
    return {
      dataItems: [],
      items: [],
      visible: false,
      activeItem: { label: "", description: "", icon: "", id: "", arg: "" },
      middlePoint: this.getMiddlePoint(),
      radius: window.innerHeight * 0.25,
      itemBorderWidth: itemBorderWidth,
      itemRadius: window.innerHeight * 0.09 - itemBorderWidth,

      activeIdx: 0,

      width: 960,
      height: 500 - 29,

      color: null,
      min: null,
      oRadius: null,
      iRadius: null,
      pie: null,
      arc: null,
      arcOuter: null,
      arcPhantom: null,
      svg: null,
      shiftWidth: null,
      // g         : null,
      arcs: null,
      outerArcs: null,
      outerArcsBig: null,
      phantomArcs: null,

      d: null,
      path: null,
    };
  },
  methods: {
    setDataItems: function (val) {
      this.dataItems = JSON.parse(val);
    },

    renderChart() {
      let _this = this;

      let image_width = 40,
        image_height = 40,
        image;

      this.d = this.makeData(this.items.length);

      this.pie.startAngle((Math.PI / this.items.length) * -1).padAngle(0.005);

      this.arcs.selectAll("path").remove();
      this.arcs.selectAll("g").remove();
      this.outerArcs.selectAll("path").remove();
      this.phantomArcs.selectAll("path").remove();

      this.outerArcs
        .datum(this.d)
        .selectAll("path")
        .data(this.pie)
        .enter()
        .append("path")
        .attr("fill", "rgba(0,0,0,1)")
        .attr("d", this.arcOuter)
        .style("stroke", "white")
        .style("stroke-width", 0);

      this.arcs
        .datum(this.d)
        .selectAll("path")
        .data(this.pie)
        .enter()
        .append("path")
        .attr("fill", "rgba(0,0,0,0.3)")
        .attr("d", this.arc)
        .each(function (d, i) {
          console.log(i);
          let self = this;

          // Load image
          image = new Image();
          image.onload = function (val) {
            console.log(val);
            image_height = image.height;
            image_width = image.width;

            // Nice aber buggy...
            let dim = _this.calculateAspectRatioFit(
              image_width,
              image_height,
              _this.arc.outerRadius()() / _this.items.length,
              _this.arc.outerRadius()() - _this.arc.innerRadius()()
            );

            dim = {
              width: 50,
              height: 50,
            };

            // append
            d3.select(self.parentNode)
              .append("g")
              .attr("transform", function () {
                return "translate(" + _this.arc.centroid(d) + ")";
              })
              .append("svg:image")
              .attr("xlink:href", function () {
                return _this.getImageLoader(d.data.image);
              })
              .attr("width", dim.width)
              .attr("height", dim.height)
              .attr("x", (-1 * dim.width) / 2)
              .attr("y", (-1 * dim.height) / 2);
          };
          image.src = d.data.image;
        });
    },

    makeData(size) {
      let _this = this;

      return d3.range(size).map(function (item) {
        return {
          value: 100 / size,
          image: _this.items[item].icon,
          label: _this.items[item].label,
          id: _this.items[item].id,
        };
      });
    },

    getMiddlePoint: function () {
      var width = window.innerWidth;
      var height = window.innerHeight;
      var x = Math.round(width / 2);
      var y = Math.round(height / 2);
      return { x: x, y: y };
    },

    onMouseMove: function (event) {
      var selectedIndex = this.getIndexFromPoint({
        x: event.clientX,
        y: event.clientY,
      });

      this.activeIdx = selectedIndex;
      this.activeItem = this.items[selectedIndex];
    },

    onKeyUp: function (event) {
      if (event.key === "x") {
        this.items = [];
        mp.trigger("SelectXMenuItem", this.activeItem.id, this.activeItem.arg);
        this.dataItems = [];
      }
    },

    getIndexFromPoint(point) {
      var itemRange = (2 * Math.PI) / this.items.length;
      var diffX = this.middlePoint.x - point.x;
      var diffY = this.middlePoint.y - point.y;
      var angle = Math.atan2(diffY, diffX);
      //rotate that 0 is on top
      angle += (Math.PI * 3) / 2;
      //rotate lightly to left that circle is in the middle of the range
      angle += itemRange / 2;
      //calculate module 2pi to be in range [0, 2pi]
      angle %= 2 * Math.PI;
      return Math.floor(angle / itemRange);
    },

    getItems: function (dataItems) {
      let i = 0;
      var items = [];
      for (let item of dataItems) {
        if (!item) return;
        var angle =
          Math.PI + ((2 * Math.PI) / dataItems.length) * (dataItems.length - i);
        var x =
          this.middlePoint.x +
          this.radius * Math.sin(angle) -
          this.itemRadius / 2;
        var y =
          this.middlePoint.y +
          this.radius * Math.cos(angle) -
          this.itemRadius / 2;

        items.push({
          x: x,
          y: y,
          label: item.label,
          description: item.description,
          icon: item.icon,
          id: item.id,
          arg: item.arg,
        });
        i++;
      }
      return items;
    },

    calculateAspectRatioFit(srcWidth, srcHeight, maxWidth, maxHeight) {
      let ratio = Math.min(maxWidth / srcWidth, maxHeight / srcHeight);

      return { width: srcWidth * ratio, height: srcHeight * ratio };
    },
  },
  mounted() {
    window.vm.XMenu = this;
    this.color = d3.interpolateRainbow;

    this.min = Math.min(this.width, this.height);

    // this.oRadius = this.min / 2 * 1.5
    // this.iRadius = this.min / 2
    this.oRadius = window.innerHeight * 0.295;
    this.iRadius = window.innerHeight * 0.19;

    this.width = window.innerWidth;
    this.height = window.innerHeight;

    this.pie = d3
      .pie()
      .value(function (d) {
        return d.value;
      })
      .sort(null);

    this.arc = d3.arc().outerRadius(this.oRadius).innerRadius(this.iRadius);

    this.arcOuter = d3
      .arc()
      .outerRadius(this.oRadius + 4)
      .innerRadius(this.iRadius + (this.oRadius - this.iRadius - 1));

    this.arcOuterBig = d3
      .arc()
      .outerRadius(this.oRadius + 8)
      .innerRadius(this.iRadius + (this.oRadius - this.iRadius - 1));

    this.arcPhantom = d3
      .arc()
      .outerRadius(this.oRadius + 20)
      .innerRadius(0);

    this.svg = d3
      .selectAll("#XCircleMenu")
      .append("svg")
      .attr("width", this.width)
      .attr("height", this.height);

    this.d = this.makeData(this.dataItems.length);

    let _w = this.width,
      _h = this.height;

    this.outerArcs = this.svg
      .append("g")
      .attr("class", "outer-arc")
      .attr("transform", function () {
        let shiftWidth = null;
        if (window.innerWidth >= 960) shiftWidth = _w / 2 + 3;
        if (window.innerWidth <= 960) shiftWidth = _w / 3 + 3;

        return "translate(" + shiftWidth + "," + (_h / 2 - 58.5) + ")";
      });

    this.arcs = this.svg
      .append("g")
      .attr("class", "arc")
      .attr("transform", function () {
        let shiftWidth = null;
        if (window.innerWidth >= 960) shiftWidth = _w / 2 + 3;
        if (window.innerWidth <= 960) shiftWidth = _w / 3 + 3;

        return "translate(" + shiftWidth + "," + (_h / 2 - 58.5) + ")";
      });

    this.phantomArcs = this.svg
      .append("g")
      .attr("class", "phantom-arc")
      .attr("transform", function () {
        let shiftWidth = null;
        if (window.innerWidth >= 960) shiftWidth = _w / 2 + 3;
        if (window.innerWidth <= 960) shiftWidth = _w / 3 + 3;

        return "translate(" + shiftWidth + 2 + "," + (_h / 2 - 58.5) + ")";
      });

    this.renderChart();
  },
  watch: {
    dataItems: function (val) {
      this.items = this.getItems(val);

      this.renderChart();
    },
    items: function (val) {
      this.visible = val.length > 0;
    },
    visible: function (val) {
      if (val) {
        window.addEventListener("mousemove", this.onMouseMove);
        window.addEventListener("keyup", this.onKeyUp);
      } else {
        window.removeEventListener("mousemove", this.onMouseMove);
        window.removeEventListener("keyup", this.onKeyUp);
      }
    },
    activeItem: function (val) {
      console.log(val);
      let _this = this;

      this.arcs
        .selectAll("path")
        .data(this.pie)
        .each(function (d, i) {
          console.log(d);
          if (i === _this.activeIdx) {
            d3.select(this).attr("fill", "rgba(214,71,0,0.3)");
          } else {
            d3.select(this).attr("fill", "rgba(0,0,0,0.3)");
          }
        });

      this.outerArcs
        .selectAll("path")
        .data(this.pie)
        .each(function (d, i) {
          if (i === _this.activeIdx) {
            d3.select(this)
              .attr("fill", "rgba(214,71,0,1)")
              .attr("d", _this.arcOuterBig);
          } else {
            d3.select(this)
              .attr("fill", "rgba(0,0,0,1)")
              .attr("d", _this.arcOuter);
          }
        });
    },
  },
};
</script>

<style scoped>
.circleMenu div div {
  background-color: #1a1a1a;
  border: none;
  border-radius: 200px;
  position: absolute;
  overflow: hidden;
  opacity: 0.9;
  display: flex;
  justify-content: center;
  align-items: center;
}

.circleMenu div div img {
  width: 60%;
  height: auto;
  opacity: inherit;
}

.circleMenu div div.active {
  background-color: #44849e;
}

.circleMenu #circleMenuDescription,
.circleMenu #circleMenuLabel {
  position: absolute;
  text-align: center;
  width: 350px;
  height: 50px;
  margin-top: 5%;
  color: #fff;
  text-shadow: -1px 0 #000, 0 1px #000, 1px 0 #000, 0 -1px #000;
  -webkit-touch-callout: none;
  user-select: none;
}
</style>
