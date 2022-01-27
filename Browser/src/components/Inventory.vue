<template>
  <div
    id="inventory"
    class="window"
    v-if="visible"
    @click="closePopSmoke($event)"
  >
    <div class="wrapper" id="inventorywrapper">
      <div class="head">
        <h1>Inventory</h1>
        <div style="margin-left: 35%" class="head-dot headdotactive"></div>
        <div class="head-dot headdotactive"></div>
        <div class="head-dot headdotactive"></div>
        <div class="head-dot headdotactive"></div>
        <div class="head-dot headdotactive"></div>
        <div class="head-dot headdotactive"></div>
        <div class="head-dot"></div>
        <div class="head-dot"></div>
        <div class="head-dot"></div>
        <div class="head-dot"></div>
        <h2>{{ GetInvenvtoryWeight }}/{{ MaxKg }} kg</h2>
      </div>
      <div class="items-wrapper">
        <div
          :class="
            activeItem?.Id == item.Id && fromInv ? 'item itemactive' : 'item'
          "
          v-for="(item, index) in items"
          :key="item.Id"
          :id="'inv-' + index"
          @click="openPopSmoke($event, item)"
          @mousedown.left="mouseDown"
          @mousedown.right="mouseDownR"
          @mouseover="mouseOver"
          @contextmenu.prevent
        >
          <img
            style="pointer-events: none"
            :src="require(`../assets/img/Items/${item.Name.toLowerCase()}.png`)"
            class="item-img"
            v-if="item.Name != ''"
          />
          <div class="amount-c" v-if="item.Name != ''">
            <h3>{{ item.Amount }}</h3>
          </div>
        </div>
      </div>
    </div>
    <div class="wrapper" id="trunkwrapper" v-if="TrunkVisible">
      <div class="head">
        <h1 style="width: 50%">Kofferraum</h1>
        <div style="margin-left: 15%" class="head-dot headdotactive"></div>
        <div class="head-dot headdotactive"></div>
        <div class="head-dot headdotactive"></div>
        <div class="head-dot headdotactive"></div>
        <div class="head-dot headdotactive"></div>
        <div class="head-dot headdotactive"></div>
        <div class="head-dot"></div>
        <div class="head-dot"></div>
        <div class="head-dot"></div>
        <div class="head-dot"></div>
        <h2 style="width: 40%">{{ GetTrunkWeight }}/{{ TrunkMaxKg }} kg</h2>
      </div>
      <div class="items-wrapper">
        <div
          :class="
            activeItem?.Id == item.Id && !fromInv ? 'item itemactive' : 'item'
          "
          v-for="(item, index) in trunk"
          :key="item.Id"
          :id="'trunk-' + index"
          @mousedown.left="mouseDown"
          @mousedown.right="mouseDownR"
          @mouseover="mouseOver"
          @contextmenu.prevent
        >
          <img
            style="pointer-events: none"
            :src="require(`../assets/img/Items/${item.Name.toLowerCase()}.png`)"
            class="item-img"
            v-if="item.Name != ''"
          />
          <div class="amount-c" v-if="item.Name != ''">
            <h3>{{ item.Amount }}</h3>
          </div>
        </div>
      </div>
    </div>
    <div class="infobox" v-if="activeItem != null && activeItem.Name != ''">
      <div class="leftimgc">
        <img src="../assets/img/Inventory/info.svg" class="leftimg" />
      </div>
      <h1 class="infohead">{{ activeItem.Name }}</h1>
      <h3 class="infotxt">{{ activeItem.Desc }}</h3>
    </div>
    <div id="popsmoke-wrapper" class="NOCLOSEPOPSMOKE">
      <div class="popsmoke-btn popbtn1 NOCLOSEPOPSMOKE" @click="UseItem()">
        <img
          src="../assets/img/Inventory/done.svg"
          class="popsmokeimg NOCLOSEPOPSMOKE np"
        />
        <h6 class="np">Benutzen</h6>
      </div>
      <div class="popsmoke-btn popbtn2 NOCLOSEPOPSMOKE" @click="ThrowItem()">
        <img
          src="../assets/img/Inventory/trash.svg"
          class="popsmokeimg NOCLOSEPOPSMOKE np"
        />
        <h6 class="np">Wegwerfen</h6>
      </div>
    </div>
    <div class="plopped-wrapper" v-if="ploppedOpen">
      <div class="plopup">
        <img src="../assets/img/Inventory/popup.png" class="popup-img" />
        <img
          class="popup-exit"
          src="../assets/img/Inventory/exit.svg"
          @click="closePloppedMenu()"
        />
        <div class="plop-wrapper">
          <div class="plopped-item-c">
            <img
              :src="
                require(`../assets/img/Items/${activeItem.Name.toLowerCase()}.png`)
              "
              class="item-img"
            />
            <div class="amount-c">
              <h3>{{ activeItem.Amount }}</h3>
            </div>
          </div>
          <div class="plopped-amount">
            <img
              style="transform: rotate(180deg)"
              src="../assets/img/Inventory/arrow-r.svg"
              class="arrow-img"
              @click="ploppedAmount > 1 ? ploppedAmount-- : (ploppedAmount = 1)"
            />
            <input type="text" maxlength="3" v-model="ploppedAmount" />
            <img
              src="../assets/img/Inventory/arrow-r.svg"
              class="arrow-img"
              @click="
                ploppedAmount < activeItem.Amount
                  ? ploppedAmount++
                  : (ploppedAmount = activeItem.Amount)
              "
            />
          </div>
        </div>
        <h5 @click="finishPlopped()">Continue</h5>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "Inventory",
  data() {
    return {
      visible: false,
      items: [],
      trunk: [],
      targetDrag: null,
      hoveredItem: null,
      targetTooltip: null,
      itemElement: null,
      clonedItemElement: null,
      rmb: false,
      inventoryOffset: {
        x: null,
        y: null,
      },
      mouseOffset: {
        x: null,
        y: null,
      },
      mousePosition: {
        x: null,
        y: null,
      },
      activeItem: null,
      ploppedOpen: false,
      ploppedAmount: 1,
      targetSlot: -1,
      targetType: false,
      fromInv: true,
      ploppedType: 0,
      TrunkVisible: false,
      MaxKg: 40,
      TrunkMaxKg: 150,
      TrunkVehileID: -1,
    };
  },
  methods: {
    Show(show, _data) {
      this.visible = show;
      if (show) {
        let alldata = JSON.parse(_data);
        let data = alldata.Inventory.Items;
        let data2 = alldata.Trunk?.Items;
        this.MaxKg = alldata.Inventory.MaxKG;
        this.TrunkMaxKg = alldata.Trunk?.MaxKG;
        this.TrunkVehileID = alldata.VehId;
        if (data2 != null) this.TrunkVisible = true;
        for (let i = 0; i < 20; i++) {
          let item = data.filter((e) => e.Slot == i + 1)[0];
          if ((item != null) | undefined) {
            let item2 = {
              Id: i + 1,
              Name: item.Model.Name,
              Desc: item.Model.Info,
              Amount: item.Amount,
              Weight: item.Model.Weight,
              Slot: item.Slot,
              MaxAmount: item.Model.MaxAmount,
            };
            this.items[i] = item2;
          }

          if (data2 == null) continue;
          let trunkitem = data2.filter((e) => e.Slot == i + 1)[0];
          if ((trunkitem != null) | undefined) {
            let trunkitem2 = {
              Id: i + 1,
              Name: trunkitem.Model.Name,
              Desc: trunkitem.Model.Info,
              Amount: trunkitem.Amount,
              Weight: trunkitem.Model.Weight,
              Slot: trunkitem.Slot,
              MaxAmount: trunkitem.Model.MaxAmount,
            };
            this.trunk[i] = trunkitem2;
          }
        }
        setTimeout(() => {
          document
            .getElementById("inventorywrapper")
            .classList.add("wrapper-anim");
          document
            .getElementById("trunkwrapper")
            ?.classList.add("wrapper-anim");
        }, 10);
      } else {
        this.ResetItems();
        this.activeItem = null;
        this.ploppedOpen = false;
        this.targetSlot = -1;
        this.ploppedType = 0;
        setTimeout(() => {
          document
            .getElementById("inventorywrapper")
            .classList.remove("wrapper-anim");
          document
            .getElementById("trunkwrapper")
            ?.classList.remove("wrapper-anim");
        }, 10);
      }
    },
    ResetItems() {
      this.TrunkVehileID = -1;
      this.TrunkVisible = false;
      for (let i = 1; i <= 20; i++) {
        this.items[i - 1] = {
          Id: i,
          Name: "",
          Desc: "",
          Weight: 0,
          Amount: 0,
          Slot: i,
          MaxAmount: 0,
        };

        this.trunk[i - 1] = {
          Id: i,
          Name: "",
          Desc: "",
          Weight: 0,
          Amount: 0,
          Slot: i,
          MaxAmount: 0,
        };
      }
    },
    GetItemInSlot(slot) {
      return this.items.filter((e) => e.Slot == slot)[0];
    },
    getMousePosition() {
      return { top: `${this.mouseY}px`, left: `${this.mouseX}px` };
    },
    onMouseMove: function (event) {
      this.mouseClientX = event.clientX;
      this.mouseClientY = event.clientY;
      if (this.clonedItemElement == null) {
        this.mouseOffset.x = this.inventoryOffset.x;
        this.mouseOffset.y = this.inventoryOffset.y;
      }
      this.mousePosition.x = event.clientX - this.mouseOffset.x;
      this.mousePosition.y = event.clientY - this.mouseOffset.y;
      if (this.clonedItemElement != null) {
        this.clonedItemElement.style.transform = `translate3d(${this.mousePosition.x}px, ${this.mousePosition.y}px, 0px)`;
      }
    },
    mouseOver(e) {
      if (this.targetDrag === null) {
        return;
      }

      this.hoveredItem = parseInt(
        e.target.id.includes("inv")
          ? e.target.id.slice(4)
          : e.target.id.slice(6)
      );
    },
    mouseDown(e, right) {
      console.log("mousedown");
      if (!e.target.id || this.activeItem != null) return;

      this.mouseX = e.clientX;
      this.mouseY = e.clientY;
      let id = -1;
      if (e.target.id.includes("inv")) {
        this.fromInv = true;
        id = e.target.id.slice(4);
      } else if (e.target.id.includes("trunk")) {
        this.fromInv = false;
        id = e.target.id.slice(6);
      }
      if ((this.fromInv ? this.items[id].Name : this.trunk[id].Name) == "")
        return;
      if (right) this.rmb = true;
      this.targetDrag = parseInt(id);
      this.activeItem = this.fromInv ? this.items[id] : this.trunk[id];
      this.mouseUpBind = this.mouseUp.bind(this);
      window.addEventListener("mouseup", this.mouseUpBind);

      let itemElement = e.target;
      this.itemElement = itemElement;

      let width = itemElement.offsetWidth;
      let height = itemElement.offsetHeight;

      var el = document.getElementById(
        e.target.id.includes("inv") ? "inventorywrapper" : "trunkwrapper"
      );
      var rect = el.getBoundingClientRect();
      this.mouseOffset.x = width / 2 + rect.left;
      this.mouseOffset.y = height / 2 + rect.top * 3.4;

      this.mousePosition.x = e.clientX - this.mouseOffset.x;
      this.mousePosition.y = e.clientY - this.mouseOffset.y;

      this.clonedItemElement = itemElement.cloneNode(true);
      this.clonedItemElement.className += " cloned np";
      this.clonedItemElement.style.position = "absolute";
      this.clonedItemElement.style.transform = `translate3d(${this.mousePosition.x}px,${this.mousePosition.y}px,0px)`;
      this.clonedItemElement.style.width = width + "px";
      this.clonedItemElement.style.height = height + "px";
      this.clonedItemElement.style.zIndex = "3";

      if (!this.itemElement.classList.contains("dragged")) {
        this.itemElement.classList.add("dragged");
      }

      document
        .getElementById(
          e.target.id.includes("inv") ? "inventorywrapper" : "trunkwrapper"
        )
        .appendChild(this.clonedItemElement);
    },
    mouseDownR(e) {
      this.mouseDown(e, true);
      console.log("mouseDown RIGHT");
    },
    getElementBehindClonedElement: function (x, y) {
      for (let el of document.elementsFromPoint(x, y))
        if (el.classList.contains("placeholder")) return el;

      return null;
    },
    mouseUp(e) {
      console.log("mouseup");

      this.hoveredItem = null;
      window.removeEventListener("mouseup", this.mouseUpBind);
      this.clonedItemElement.parentNode.removeChild(this.clonedItemElement);
      this.clonedItemElement = null;
      document
        .getElementById((this.fromInv ? "inv-" : "trunk-") + this.targetDrag)
        .classList.remove("dragged");
      if (!e.target.classList.contains("item")) {
        this.rmb = false;
        return;
      }

      if (this.targetDrag === null) {
        this.rmb = false;
        return;
      }

      let id = -1;
      let targetType = false; // false = INV true = TRUNK
      if (e.target.id.includes("inv")) {
        id = e.target.id.slice(4);
        targetType = false;
      } else {
        id = e.target.id.slice(6);
        targetType = true;
      }

      const oldIndex = this.targetDrag;
      const newIndex = parseInt(id);

      if (
        (this.fromInv
          ? this.items[oldIndex].Name
          : this.trunk[oldIndex].Name) == "" ||
        isNaN(newIndex) ||
        (oldIndex === newIndex && this.fromInv == !targetType)
      ) {
        this.targetSlot = -1;
        this.activeItem = null;
        this.rmb = false;
        return;
      }

      if (this.rmb) {
        if (
          (targetType
            ? this.items[newIndex].Name
            : this.trunk[newIndex].Name) != ""
        )
          return;
        this.rmb = false;
        this.openPloppedMenu(this.items[newIndex]);
        this.ploppedType = 0;
        this.targetType = targetType;
        return;
      }

      this.activeItem = null;
      this.targetDrag = null;
      let oldItem = null;
      let newItem = null;
      if (this.fromInv) {
        oldItem = this.items[oldIndex] ? { ...this.items[oldIndex] } : null;
        newItem = this.items[newIndex] ? { ...this.items[newIndex] } : null;
      } else {
        oldItem = this.trunk[oldIndex] ? { ...this.trunk[oldIndex] } : null;
        newItem = this.trunk[newIndex] ? { ...this.trunk[newIndex] } : null;
      }
      if (!oldItem) return;

      if (this.fromInv && e.target.id.includes("inv")) {
        mp.trigger("Inventory:MoveAllToSlot", oldItem.Slot, newItem.Slot);
        if (oldItem.Name == newItem.Name) {
          let diff = newItem.MaxAmount - newItem.Amount;
          if (diff < oldItem.Amount) {
            this.items[newIndex].Amount += diff;
            this.items[oldIndex].Amount -= diff;
            if (this.items[oldIndex].Amount < 1)
              this.items[oldIndex] = {
                Id: oldItem.Id,
                Name: "",
                Desc: "",
                Weight: 0,
                Amount: 0,
                Slot: oldItem.Slot,
                MaxAmount: 0,
              };
          } else {
            this.items[newIndex].Amount += oldItem.Amount;
            this.items[oldIndex] = {
              Id: oldItem.Id,
              Name: "",
              Desc: "",
              Weight: 0,
              Amount: 0,
              Slot: oldItem.Slot,
              MaxAmount: 0,
            };
          }
        } else {
          this.items[newIndex] = {
            Id: newItem.Id,
            Name: oldItem.Name,
            Desc: oldItem.Desc,
            Weight: oldItem.Weight,
            Amount: oldItem.Amount,
            Slot: newItem.Slot,
            MaxAmount: oldItem.MaxAmount,
          };
          this.items[oldIndex] = {
            Id: oldItem.Id,
            Name: newItem.Name,
            Desc: newItem.Desc,
            Weight: newItem.Amount,
            Amount: newItem.Amount,
            Slot: oldItem.Slot,
            MaxAmount: newItem.MaxAmount,
          };
        }
      } else if (!this.fromInv && e.target.id.includes("trunk")) {
        mp.trigger(
          "Inventory:MoveAllToSlotTrunk",
          this.TrunkVehileID,
          oldItem.Slot,
          newItem.Slot
        );
        if (oldItem.Name == newItem.Name) {
          let diff = newItem.MaxAmount - newItem.Amount;
          if (diff < oldItem.Amount) {
            this.trunk[newIndex].Amount += diff;
            this.trunk[oldIndex].Amount -= diff;
            if (this.trunk[oldIndex].Amount < 1)
              this.trunk[oldIndex] = {
                Id: oldItem.Id,
                Name: "",
                Desc: "",
                Weight: 0,
                Amount: 0,
                Slot: oldItem.Slot,
                MaxAmount: 0,
              };
          } else {
            this.trunk[newIndex].Amount += oldItem.Amount;
            this.trunk[oldIndex] = {
              Id: oldItem.Id,
              Name: "",
              Desc: "",
              Weight: 0,
              Amount: 0,
              Slot: oldItem.Slot,
              MaxAmount: 0,
            };
          }
        } else {
          this.trunk[newIndex] = {
            Id: newItem.Id,
            Name: oldItem.Name,
            Desc: oldItem.Desc,
            Weight: oldItem.Weight,
            Amount: oldItem.Amount,
            Slot: newItem.Slot,
            MaxAmount: oldItem.MaxAmount,
          };
          this.trunk[oldIndex] = {
            Id: oldItem.Id,
            Name: newItem.Name,
            Desc: newItem.Desc,
            Weight: newItem.Amount,
            Amount: newItem.Amount,
            Slot: oldItem.Slot,
            MaxAmount: newItem.MaxAmount,
          };
        }
      } else {
        mp.trigger(
          "Inventory:MoveItemToContainer",
          this.TrunkVehileID,
          oldItem.Slot,
          newItem.Slot,
          this.fromInv
        );
        if (this.fromInv) {
          if (this.trunk[newIndex].Name != "") return;
          this.trunk[newIndex] = {
            Id: newItem.Id,
            Name: oldItem.Name,
            Desc: oldItem.Desc,
            Weight: oldItem.Weight,
            Amount: oldItem.Amount,
            Slot: newItem.Slot,
            MaxAmount: oldItem.MaxAmount,
          };
          this.items[oldIndex] = {
            Id: oldItem.Id,
            Name: "",
            Desc: "",
            Weight: 0,
            Amount: 0,
            Slot: oldItem.Slot,
            MaxAmount: 0,
          };
        } else {
          if (this.items[newIndex].Name != "") return;
          this.items[newIndex] = {
            Id: newItem.Id,
            Name: oldItem.Name,
            Desc: oldItem.Desc,
            Weight: oldItem.Weight,
            Amount: oldItem.Amount,
            Slot: newItem.Slot,
            MaxAmount: oldItem.MaxAmount,
          };
          this.trunk[oldIndex] = {
            Id: oldItem.Id,
            Name: "",
            Desc: "",
            Weight: 0,
            Amount: 0,
            Slot: oldItem.Slot,
            MaxAmount: 0,
          };
        }
      }
    },
    openPloppedMenu(newSlot) {
      this.ploppedOpen = true;
      this.targetSlot = newSlot.Slot;
      this.ploppedType = 0;
    },
    closePloppedMenu() {
      this.ploppedOpen = false;
      this.ploppedAmount = 1;
    },
    finishPlopped() {
      switch (this.ploppedType) {
        case 0: {
          let amount = parseInt(this.ploppedAmount);
          this.closePloppedMenu();
          if (
            isNaN(amount) ||
            amount < 1 ||
            amount > this.activeItem.Amount ||
            this.targetSlot < 0
          )
            return;
          if (this.activeItem.Amount == amount) {
            if (this.fromInv && !this.targetType) {
              this.items[this.targetSlot] = {
                Id: this.items[this.targetSlot].Id,
                Name: this.activeItem.Name,
                Desc: this.activeItem.Desc,
                Weight: this.activeItem.Weight,
                Amount: this.activeItem.Amount,
                Slot: this.targetSlot,
                MaxAmount: this.activeItem.MaxAmount,
              };
              (this.items[this.activeItem.Id] = {
                Id: this.activeItem.Id,
                Name: "",
                Desc: "",
                Weight: 0,
                Amount: 0,
                Slot: this.activeItem.Slot,
                MaxAmount: 0,
              }),
                mp.trigger(
                  "Inventory:MoveAllToSlot",
                  this.activeItem.Slot,
                  this.targetSlot
                );
            } else {
              this.trunk[this.targetSlot] = {
                Id: this.trunk[this.targetSlot].Id,
                Name: this.activeItem.Name,
                Desc: this.activeItem.Desc,
                Weight: this.activeItem.Weight,
                Amount: this.activeItem.Amount,
                Slot: this.targetSlot,
                MaxAmount: this.activeItem.MaxAmount,
              };
              (this.trunk[this.activeItem.Slot.Id] = {
                Id: this.activeItem.Id,
                Name: "",
                Desc: "",
                Weight: 0,
                Amount: 0,
                Slot: this.activeItem.Slot,
                MaxAmount: 0,
              }),
                mp.trigger(
                  "Inventory:MoveAllToSlotTrunk",
                  this.TrunkVehileID,
                  this.activeItem.Slot,
                  this.targetSlot
                );
            }
          } else {
            if (this.fromInv && !this.targetType) {
              let item = this.items.find((e) => e.Slot == this.targetSlot);
              this.items[this.activeItem.Id - 1].Amount -= amount;
              this.items[item.Id - 1] = {
                Id: item.Id,
                Name: this.activeItem.Name,
                Desc: this.activeItem.Desc,
                Weight: this.activeItem.Weight,
                Amount: amount,
                Slot: this.targetSlot,
                MaxAmount: this.activeItem.MaxAmount,
              };

              mp.trigger(
                "Inventory:MoveAmountToSlot",
                this.activeItem.Slot,
                this.targetSlot,
                amount
              );
            } else {
              let item = this.trunk.find((e) => e.Slot == this.targetSlot);
              this.trunk[this.activeItem.Id - 1].Amount -= amount;
              this.trunk[item.Id - 1] = {
                Id: item.Id,
                Name: this.activeItem.Name,
                Desc: this.activeItem.Desc,
                Weight: this.activeItem.Weight,
                Amount: amount,
                Slot: this.targetSlot,
                MaxAmount: this.activeItem.MaxAmount,
              };

              mp.trigger(
                "Inventory:MoveAmountToSlotTrunk",
                this.TrunkVehileID,
                this.activeItem.Slot,
                this.targetSlot,
                amount
              );
            }
          }
          break;
        }
        case 1: {
          let amount = parseInt(this.ploppedAmount);
          this.closePloppedMenu();
          if (
            isNaN(amount) ||
            amount < 1 ||
            amount > this.items[this.activeItem.Id].Amount ||
            this.activeItem == null ||
            this.activeItem.Name == ""
          )
            return;
          if (this.items[this.activeItem.Id].Amount == amount)
            this.items[this.activeItem.Id] = {
              Id: this.activeItem.Id,
              Name: "",
              Weight: 0,
              Amount: 0,
              Slot: this.activeItem.Slot,
            };
          else this.items[this.activeItem.Id].Amount -= amount;
          mp.trigger("Inventory:ThrowItem", this.activeItem.Slot, amount);
          break;
        }
      }
    },
    openPopSmoke(e, item) {
      console.log("click");
      if (item.Name == "") return;
      console.log("e: " + e);
      this.activeItem = item;
      let menu = document.getElementById("popsmoke-wrapper");
      menu.style.top = e.clientY - 25 + "px";
      menu.style.left = e.clientX + "px";
      menu.style.display = "block";
    },
    closePopSmoke(e) {
      let el = document.elementFromPoint(e.clientX, e.clientY);
      if (el.className.includes("NOCLOSEPOPSMOKE")) return;
      document.getElementById("popsmoke-wrapper").style.display = "none";
      if (!this.ploppedOpen) this.activeItem = null;
    },
    UseItem() {
      if (this.activeItem == null || this.activeItem.Name == "") return;

      mp.trigger("Inventory:UseItem", this.activeItem.Slot);
    },
    ThrowItem() {
      this.ploppedOpen = true;
      this.ploppedType = 1;
    },
  },
  mounted() {
    window.addEventListener("mousemove", this.onMouseMove);
    window.vm.Inventory = this;

    this.ResetItems();
    this.activeItem = null;
    this.ploppedOpen = false;
    this.targetSlot = -1;
    this.ploppedType = 0;
  },
  watch: {
    items: {
      handler(val) {
        if (val.length > 20) mp.trigger("Inventory:Reload");
      },
      deep: true,
    },
  },
  computed: {
    GetInvenvtoryWeight() {
      let result = 0;
      this.items.forEach((e) => (result += e.Weight * e.Amount));
      return result;
    },
    GetTrunkWeight() {
      let result = 0;
      this.trunk.forEach((e) => (result += e.Weight * e.Amount));
      return result;
    },
  },
};
</script>

<style scoped>
#inventory {
  background: linear-gradient(
    90deg,
    rgba(0, 164, 255, 1) 0%,
    rgba(0, 0, 0, 0.4) 71%
  );
  box-shadow: inset 0px 0px 150px 5px #000000;
}
#inventory::before {
  content: "";
  position: absolute;
  width: 200%;
  height: 200%;
  top: -50%;
  left: -50%;
  z-index: 1;
  background-image: radial-gradient(#3d3d3d3a 7%, transparent 7%),
    radial-gradient(#3d3d3d3a 7%, transparent 7%);
  background-position: 0 0, 20px 20px;
  background-size: 40px 40px;
  transform: rotate(45deg);
}
.wrapper {
  position: absolute;
  width: 25%;
  height: 55%;
  top: 65%; /* 65% */
  opacity: 0;
  left: 30%;
  transform: translate(-50%, -50%);
  z-index: 1;
  overflow: visible;
}
#trunkwrapper {
  position: absolute;
  width: 25%;
  height: 55%;
  top: 65%; /* 65% */
  opacity: 0;
  left: 60%;
  transform: translate(-50%, -50%);
  z-index: 1;
  overflow: visible;
}
.head {
  width: 100%;
  height: 15%;
  background-image: url(../assets/img/Inventory/banner.png);
  background-position: center top;
  background-size: 100%;
  border-radius: 10px;
  display: flex;
  align-items: center;
}
h1 {
  font-size: 23px;
  font-weight: 400;
  color: rgb(212, 212, 212);
  margin-left: 8%;
  float: left;
}
.head-dot {
  width: 3px;
  height: 13%;
  background: white;
  margin-right: 1%;
  border-radius: 50px;
  float: right;
  flex-shrink: 0;
}
.headdotactive {
  background: rgba(0, 164, 255, 1);
}
h2 {
  font-size: 19px;
  font-weight: 400;
  color: rgb(212, 212, 212);
  margin-left: 2%;
  float: left;
}
.items-wrapper {
  width: 100%;
  height: 70%;
  margin-top: 5%;
  overflow: visible;
}
.item {
  position: relative;
  display: flex;
  width: 19%;
  height: 22%;
  background: rgba(22, 24, 26, 0.85);
  border-radius: 5px;
  float: left;
  margin: 0.5%;
  justify-content: center;
  align-items: center;
  z-index: 3;
}
.item-img {
  width: 60%;
  height: auto;
}
.dragged {
  opacity: 0.5;
}
.amount-c {
  position: absolute;
  top: 4%;
  right: 3%;
  width: fit-content;
  height: 20%;
  background: #24b1dd;
  border-radius: 5px 5px 5px 20px;
  padding-left: 15%;
}
h3 {
  font-size: 14px;
  margin: 0;
  margin-right: 25%;
  float: right;
}
.plopped-wrapper {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  z-index: 2;
}
.plopup {
  position: absolute;
  top: 45%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 10.8%;
  height: 25%;
  background: #24b1dd;
  border-radius: 10px;
  text-align: center;
}
.popup-img {
  width: 100%;
  height: auto;
  border-radius: 10px;
}
.popup-exit {
  position: absolute;
  top: 3%;
  right: 3%;
  width: 12%;
  height: auto;
  z-index: 2;
}
.plop-wrapper {
  position: absolute;
  top: 0;
  right: 0;
  width: 100%;
  height: 84%;
  text-align: center;
}
.plopped-item-c {
  position: relative;
  display: flex;
  width: 45%;
  height: 40%;
  margin-top: 20%;
  margin-left: 27.5%;
  background: #2d5157bb;
  border-radius: 5px;
  align-items: center;
  justify-content: center;
}
.plopped-amount {
  display: flex;
  width: 60%;
  height: 15%;
  text-align: center;
  background: #2d5157bb;
  margin-top: 10%;
  margin-left: 20%;
  border: none;
  border-radius: 10px;
  align-items: center;
}
h4 {
  width: 45%;
  height: 50%;
  text-align: center;
}
.arrow-img {
  width: auto;
  height: 60%;
  margin-left: 5%;
}
h5 {
  height: 20%;
  width: 100%;
  margin-top: -3%;
  padding-top: 6%;
  z-index: 4;
}
input {
  width: 50%;
  height: 80%;
  background: transparent;
  border: none;
  text-align: center;
}
#popsmoke-wrapper {
  position: absolute;
  display: none;
  top: 50px;
  left: 50px;
  width: 7%;
  height: 6%;
  z-index: 2;
}
.popsmoke-btn {
  position: relative;
  display: flex;
  align-items: center;
  width: 100%;
  height: 45%;
  border: none;
}
.popbtn1 {
  background: linear-gradient(
    0deg,
    rgba(5, 139, 0, 0.75) 0%,
    rgba(7, 212, 0, 0.75) 100%
  );
  border-radius: 5px 25px 5px 5px;
}
.popbtn2 {
  margin-top: 4%;
  background: linear-gradient(
    180deg,
    rgba(207, 0, 0, 0.75) 0%,
    rgba(230, 0, 0, 0.75) 100%
  );
  border-radius: 5px 5px 25px 5px;
}
h6 {
  margin-left: 5%;
  font-size: 17px;
  font-weight: 400;
}
.popsmokeimg {
  margin-left: 5%;
  width: auto;
  height: 65%;
}
.infobox {
  position: absolute;
  bottom: 2%;
  left: 2%;
  width: 25%;
  height: 9%;
  background: #0b0b0b;
  border-radius: 5px 5px 5px 40px;
  z-index: 2;
  background-image: url(../assets/img/Inventory/infobackground.png);
  background-position: top left;
  background-size: 100%;
}
.leftimgc {
  width: 18%;
  height: 100%;
  background: #0b0b0b;
  float: left;
  margin-right: 7%;
}
.leftimg {
  margin-left: 27.5%;
  margin-top: 30%;
  width: 45%;
  height: auto;
}
.infohead {
  font-weight: 500;
  font-size: 20px;
  margin: 0;
  margin-top: 3%;
}
.infotxt {
  position: relative;
  display: block;
  width: 70%;
  font-weight: 400;
  font-size: 15px;
  margin: 0;
  margin-top: 2%;
  text-align: left;
  float: left;
}
.itemactive {
  background: #16738e;
  box-shadow: inset 0px 0px 30px -5px #000000;
}
.wrapper-anim {
  animation: slideup 300ms linear 1;
  animation-fill-mode: forwards;
}

@keyframes slideup {
  0% {
    opacity: 0;
    top: 65%;
  }

  100% {
    opacity: 1;
    top: 48%;
  }
}
</style>
