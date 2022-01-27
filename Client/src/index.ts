enum ServerEvents {
    // Login
    PlayerLogin = 'PlayerLogin'
}

enum ClientEvents {
    // Windows
    ShowComponent = 'ShowComponent',
    // Hud 
    SetLocation = 'SetLocation',
    SetMoney = 'SetMoney',
    SetAduty = 'SetAduty,',
    SetPlayers = 'SetPlayers',
    DisplayInteraction = 'DisplayInteraction',
    HideInteraction = 'HideInteraction',
    SetData = 'SetHudData',
    DisplayAnnounce = 'DisplayAnnounce',
    DisplayNotify = 'DisplayNotify',
    DisplayProgress = 'DisplayProgress',
    // Login
    PlayerLogin = 'PlayerLogin',
    LoginDisplayError = 'LoginDisplayError',
    LoginFinished = 'LoginFinished',
    RegisterFinished = 'RegisterFinished',
    // Creator
    CreatorUpdate = 'CreatorUpdate',
    CreatorSaveChanges = 'CreatorSaveChanges',
    CreatorChangeGender = 'CreatorChangeGender',
    CreatorFinished = 'CreatorFinished',
    // Inventory
    InvMoveItem = 'Inventory:MoveItem',
    // Chat
    PlayerChat= 'PlayerChat',
    // Loading
    LoadChangeData = 'LoadChangeData',
    // Controls
    DisableAllControls = 'DisableAllControls'
}

mp.events.add('Log', (log: string) => {
    mp.console.logInfo(log)
})

let sceneryCamera: CameraMp
let PlayerDead = false



mp.nametags.enabled = false
mp.gui.chat.show(false)
mp.game.ui.displayCash(false)
mp.game.ui.displayAreaName(false)

sceneryCamera = mp.cameras.new('default', new mp.Vector3(3083.8196, 5962.94, 150.09317), new mp.Vector3(0,0,0), 42)
sceneryCamera.pointAtCoord(3110.7302, 6008.773, 136.05379)
sceneryCamera.setActive(true)
mp.game.cam.renderScriptCams(true, false, 0, true, false)
mp.game.ui.displayHud(false)
mp.game.ui.displayRadar(false)
mp.game.vehicle.defaultEngineBehaviour = false;

setInterval(() => {
    if(Browser.Instance)
        Browser.Instance.Execute(`window.vm.Hud.SetRatio(${mp.game.graphics.getScreenAspectRatio(true).toFixed(2)})`)
}, 20000)

class Browser {
    private static _instance = new Browser('package://browser/index.html')
    _Browser: BrowserMp
    Windows: Windows[]

    private constructor(url: string) {
        this._Browser = mp.browsers.new(url)
        this.Windows = new Array<Windows>()

        mp.events.add(ClientEvents.ShowComponent, this.ShowComponent)
        mp.gui.chat.activate(false)
    }

    public static get Instance(): Browser {
        return this._instance
    }

    public RegisterWindows(Windows: Windows): void {
        Browser.Instance.Windows.push(Windows)
    }

    public GetWindows(Windows: string): (Windows | undefined) {
        return Browser.Instance.Windows.find(e => e.Name == Windows)
    }

    public Execute(code: string): void {
        console.log(`Browser:Emit | ${code}`)
        this._Browser.execute(code)
    }

    public ShowComponent(comp: string, show: boolean, data: string = '{}'): void {
        Browser.Instance.GetWindows(comp)?.ShowComponent(show, data)
    }
}

class Windows {
    Name: string
    Visible: boolean
    FreezeControls: boolean

    constructor(name: string) {
        this.Name = name
        this.Visible = false
        this.FreezeControls = false
        Browser.Instance.RegisterWindows(this)
    }

    ShowComponent(show: boolean, data: string) {
        if(PlayerDead || PlayerCuffed) return
        this.Visible = show
        Browser.Instance.Execute(`window.vm.${this.Name}.Show(${show}, '${data}')`)
        if(this.Name != 'Loading' && this.Name != 'Hud') mp.gui.cursor.show(show, show)
        mp.console.logWarning(`ShowComponent:${this.Name}:${show}:${data}`)
        Browser.Instance.Execute(`window.vm.Hud.SetRatio(${mp.game.graphics.getScreenAspectRatio(true).toFixed(2)})`)
    }
}

class Hud extends Windows {
    private static _instance = new Hud()
    private SpeedoInterval: any
    private constructor() {
        super('Hud')

        this.SpeedoInterval = null

        mp.events.add(ClientEvents.SetLocation, this.SetLocation)
        mp.events.add(ClientEvents.SetMoney, this.SetMoney)
        mp.events.add(ClientEvents.SetPlayers, this.SetPlayers)
        mp.events.add(ClientEvents.SetAduty, this.SetAduty)
        mp.events.add(ClientEvents.DisplayInteraction, this.DisplayInteraction)
        mp.events.add(ClientEvents.HideInteraction, this.HideInteraction)
        mp.events.add(ClientEvents.SetData, this.SetData)
        mp.events.add(RageEnums.EventKey.PLAYER_ENTER_VEHICLE, this.PlayerEnterVehicle)
        mp.events.add(RageEnums.EventKey.PLAYER_LEAVE_VEHICLE, this.PlayerExitVehicle)
        setInterval(() => {
            Hud.Instance.SetLocation()
            Hud.Instance.SetPlayers()
        }, 5000)
    }

    static get Instance(): Hud {
        return this._instance
    }

    public SetData(data: string) {
        Browser.Instance.Execute(`window.vm.Hud.SetData('${data}')`)
    }

    public SetLocation() {
        let player = mp.players.local
        let _street = mp.game.pathfind.getStreetNameAtCoord(player.position.x, player.position.y, player.position.z, 0, 0)
        let zone = mp.game.gxt.get(mp.game.zone.getNameOfZone(player.position.x, player.position.y, player.position.z))
        let street = mp.game.ui.getStreetNameFromHashKey(_street.streetName)
        Browser.Instance.Execute(`window.vm.Hud.SetLocation('${zone}', '${street}')`)
    }

    public SetMoney(money: number) {
        Browser.Instance.Execute(`window.vm.Hud.SetMoney(${money})`)
    }

    public SetPlayers() {
        Browser.Instance.Execute(`window.vm.Hud.SetPlayers(${mp.players.length})`)
    }

    public SetAduty(duty: boolean) {
        Browser.Instance.Execute(`window.vm.Hud.SetAduty(${duty})`)
    }

    public DisplayInteraction(name: string) {
        Browser.Instance.Execute(`window.vm.Hud.ShowInteraction('${name}')`)
    }

    public HideInteraction() {
        Browser.Instance.Execute(`window.vm.Hud.RemoveInteraction()`)
    }

    public PlayerEnterVehicle(veh: VehicleMp, seat: number) {
        if(PlayerDead || PlayerCuffed) {
            mp.players.local.clearTasks();
            return;
        }
        if(Hud.Instance.SpeedoInterval != null)
            clearInterval(Hud.Instance.SpeedoInterval)

        Browser.Instance.Execute(`window.vm.Hud.ShowSpeedo(true)`)

        setInterval(() => {
            let vehicle = mp.players.local.vehicle
            if(vehicle != null){
                let engineState = vehicle.getVariable("EngineState")
                let lockState = vehicle.getVariable("LockState")
                let speed = vehicle.getSpeed();
                speed *= 3.6;
                vehicle.setEngineOn(engineState, false, true)
                Browser.Instance.Execute(`window.vm.Hud.SetVehicleSpeed(${Math.round(speed)})`)
                if(typeof(engineState) == 'boolean')
                    Browser.Instance.Execute(`window.vm.Hud.SetVehicleEngineState(${engineState})`)
                if(typeof(lockState) == 'boolean')
                    Browser.Instance.Execute(`window.vm.Hud.SetVehicleLockState(${lockState})`)
            } else {
                if(Hud.Instance.SpeedoInterval != null) {
                    clearInterval(Hud.Instance.SpeedoInterval)
                    Hud.Instance.SpeedoInterval = null
                }

                Browser.Instance.Execute(`window.vm.Hud.ShowSpeedo(false)`)
            }
        }, 20)
    }

    public PlayerExitVehicle(veh: VehicleMp, seat: number) {
        if(Hud.Instance.SpeedoInterval != null) {
            clearInterval(Hud.Instance.SpeedoInterval)
            Hud.Instance.SpeedoInterval = null
        }

        Browser.Instance.Execute(`window.vm.Hud.ShowSpeedo(false)`)
    }
}

class Login extends Windows {
    private static _instance = new Login()
    private constructor() {
        super('Login')

        mp.events.add(ClientEvents.LoginDisplayError, this.DisplayError)
        mp.events.add(ClientEvents.PlayerLogin, this.PlayerLogin)
        mp.events.add(ClientEvents.LoginFinished, this.LoginFinished)
        mp.events.add(ClientEvents.RegisterFinished, this.RegisterFinished)
    }

    static get Instance(): Login {
        return this._instance
    }

    DisplayError(data: string): void {
       Loading.Instance.ShowComponent(false, '')
        Browser.Instance.Execute(`window.vm.Login.DisplayError('${data}')`)
    }

    PlayerLogin(name: string, pass: string): void {
        Browser.Instance.GetWindows('Loading')?.ShowComponent(true, 'Bitte warten...')
        setTimeout(() => {
            mp.events.callRemote(ServerEvents.PlayerLogin, name, pass)
        }, 1000)
    }

    LoginFinished(name: string): void {
        mp.events.call(ClientEvents.LoadChangeData, 'Anmeldevorgang erfolgreich, lade Daten...')
        mp.storage.data['login_name'] = name
        setTimeout(() => {
            mp.events.call(ClientEvents.LoadChangeData, 'Erfolgreich eingeloggt!')
            setTimeout(() => {
                Login.Instance.ShowComponent(false, '')
                Loading.Instance.ShowComponent(false, '')
                Hud.Instance.ShowComponent(true, '')
                mp.game.ui.displayHud(true)
                mp.game.ui.displayRadar(true)
                mp.game.cam.renderScriptCams(false, false, 0, true, false)
                sceneryCamera.destroy()
                PlayerInvincible = false;
                mp.players.local.freezePosition(false)
            }, 1000)
        }, 700)
    }

    RegisterFinished(): void {
        mp.events.call(ClientEvents.LoadChangeData, 'Registrierung erfolgreich, logge dich nun ein.')
        setTimeout(() => {
            Browser.Instance.GetWindows('Loading')?.ShowComponent(false, '')
        }, 2500)
    }

    override ShowComponent(show: boolean, data: string): void {
        let name = mp.storage.data['login_name']
        if(name == undefined)
            name = ''
        this.Visible = show
        Browser.Instance.Execute(`window.vm.Login.Show(${show}, '${name}')`)
        mp.gui.cursor.show(show, show)
    }
}

class Creator extends Windows {
    private static _instance = new Creator()
    private constructor() {
        super('Creator')

        mp.events.add(ClientEvents.CreatorUpdate, this.CreatorUpdate)
        mp.events.add(ClientEvents.CreatorChangeGender, this.CreatorChangeGender)
        mp.events.add(ClientEvents.CreatorSaveChanges, this.CreatorSaveChanges)
        mp.events.add(ClientEvents.CreatorFinished, this.CreatorFinished)
    }

    static get Instance(): Creator {
        return this._instance
    }

    override ShowComponent(show: boolean, data: string): void {
        this.Visible = show
        Browser.Instance.ShowComponent('Hud', false, '')
        Browser.Instance.Execute(`window.vm.Creator.Show(${show}, '${data}')`)

        sceneryCamera = mp.cameras.new('default', new mp.Vector3(402.8664, -997.5515, -98.2), new mp.Vector3(0,0,0), 42)
        mp.gui.cursor.show(show, show)
        sceneryCamera.pointAtCoord(403.1, -996.4108, -98.4)
        sceneryCamera.setActive(true)
        if(show)
            mp.game.cam.renderScriptCams(true, false, 0, true, false)
        mp.game.ui.displayHud(!show)
        mp.game.ui.displayRadar(!show)
    }

    CreatorUpdate(type: string, _data: string) {
        let data = JSON.parse(_data)
        switch (type){
            case 'Hair':
                mp.players.local.setComponentVariation(2, parseInt(data.style), 0, 2);
                mp.players.local.setHairColor(parseInt(data.color), parseInt(data.color2));
                break
            case 'Parents':
                mp.players.local.setHeadBlendData(parseInt(data.mother), parseInt(data.father), 0, parseInt(data.mother), parseInt(data.father), 0, parseFloat(data.mix), parseFloat(data.mix), 0, true);
                break
            case 'Beard':
                mp.players.local.setHeadOverlay(1, parseInt(data.beard), 1, parseInt(data.beardColor), parseInt(data.beardColor));
                break
            case 'Eyebrow':
                mp.players.local.setHeadOverlay(2, parseInt(data.eyebrow), 1, parseInt(data.eyebrowColor), parseInt(data.eyebrowColor));
                break
            case 'Eye':
                mp.players.local.setEyeColor(parseInt(data.eyeColor));
                break
        }
    }

    public CreatorSaveChanges(data: string): void {
        Browser.Instance.GetWindows('Loading')?.ShowComponent(true, 'Bitte warten...')
        mp.events.callRemote('SaveCreatorChanges', data)
    }

    public CreatorChangeGender(gender: number): void {
        mp.players.local.model = (gender == 1) ? 0x705E61F2 : 0x9C9EFFD8
    }

    public CreatorFinished(): void {
        Creator.Instance.ShowComponent(false, '')
        mp.events.call(ClientEvents.LoadChangeData, 'Charakter wird erstellt...')
            setTimeout(() => {
                mp.events.call(ClientEvents.LoadChangeData, 'Charakter erfolgreich erstellt.')
                setTimeout(() => {
                    Loading.Instance.ShowComponent(false, '')
                    mp.game.cam.renderScriptCams(false, false, 0, true, false)
                    sceneryCamera.destroy()
                    mp.game.ui.displayHud(true)
                    mp.game.ui.displayRadar(true)
                    PlayerInvincible = false;
                    mp.players.local.freezePosition(false)
                }, 1000)
            }, 1000)
    }
}

class Inventory extends Windows {
    private static _instance = new Inventory()
    public OpenCoolDown: boolean
    public MoveCoolDown: boolean
    private constructor() {
        super('Inventory')
        this.OpenCoolDown = false
        this.MoveCoolDown = false

        mp.events.add('Inventory:MoveAllToSlot', this.MoveAllToSlot)
        mp.events.add('Inventory:MoveAmountToSlot', this.MoveAmountToSlot)
        mp.events.add('Inventory:MoveAllToSlotTrunk', this.MoveAllToSlotTrunk)
        mp.events.add('Inventory:MoveItemToContainer', this.MoveItemToContainer)
        mp.events.add('Inventory:MoveAmountToSlotTrunk', this.MoveAmountToSlotTrunk)
        mp.events.add('Inventory:UseItem', this.UseItem)
        mp.events.add('Inventory:ThrowItem', this.ThrowItem)
        mp.events.add('Inventory:Reload', this.ReloadInv)
        mp.keys.bind(0x49, false, this.ToggleInventory)
        mp.keys.bind(0x1B, false, this.PressEscape)
        // this.Show(true, '{"Items":[{"Model":{"Name": "Schutzweste","Info": "bla bla bla","Weight": 1,"MaxAmount": 5},"Amount": 5,"Slot": 1},{"Model":{"Name": "Schutzweste","Info": "bla bla bla","Weight": 1,"MaxAmount": 5},"Amount": 3,"Slot": 2}], "MaxKG":40, "Trunk":[{"Model":{"Name": "Schutzweste","Info": "bla bla bla","Weight": 1,"MaxAmount": 5},"Amount": 5,"Slot": 1}, {"Model":{"Name": "Schutzweste","Info": "bla bla bla","Weight": 1,"MaxAmount": 5},"Amount": 5,"Slot": 3}], "TrunkMaxKG":10}')
        // ADD EVENTS IN SERVER CLIENT AND BROWSER
        // GENERAL IMPLEMENTATION
    }

    public static get Instance(): Inventory {
        return this._instance
    }

    PressEscape(): void {
        if(!Inventory.Instance.Visible) return
        Inventory.Instance.ShowComponent(false, '')
    }

    ReloadInv(): void {
        mp.events.callRemote('GetInventoryData')
    }
    
    ToggleInventory(): void {
        if(Chat.Instance.Visible || Login.Instance.Visible || Creator.Instance.Visible || Loading.Instance.Visible || Garage.Instance.Visible) return
        if(Inventory.Instance.Visible) {
            Inventory.Instance.ShowComponent(false, '')
        } else {
            if(!Inventory.Instance.OpenCoolDown) {
                Inventory.Instance.OpenCoolDown = true
                mp.events.callRemote('GetInventoryData')
                setTimeout(() => {
                    Inventory.Instance.OpenCoolDown = false
                }, 1000)
            }
        }
    }

    UseItem(slot: number): void {
        mp.events.callRemote('UseInventoryItem', slot)
        Inventory.Instance.ShowComponent(false, '')
    }

    ThrowItem(slot: number, amount: number): void {
        mp.events.callRemote('ThrowInventoryItem', slot, amount)
        Inventory.Instance.ShowComponent(false, '')
    }

    MoveAllToSlot(oldSlot: number, newSlot: number) {
        if(Inventory.Instance.MoveCoolDown) {
            Notifications.Instance.DisplayNotify('{"Title":"Anti-Spam", "Message":"Du kannst nur 1 Item pro halber Sekunde bewegen.", "Time":3000}')
            Inventory.Instance.ShowComponent(false, '')
            return
        }
        Inventory.Instance.MoveCoolDown = true
        mp.events.callRemote('MoveAllToSlot', oldSlot, newSlot)
        setTimeout(() => {
            Inventory.Instance.MoveCoolDown = false
        }, 500)
    }

    MoveAllToSlotTrunk(vehId: number, oldSlot: number, newSlot: number) {
        if(vehId < 0) return
        if(Inventory.Instance.MoveCoolDown) {
            Notifications.Instance.DisplayNotify('{"Title":"Anti-Spam", "Message":"Du kannst nur 1 Item pro halber Sekunde bewegen.", "Time":3000}')
            Inventory.Instance.ShowComponent(false, '')
            return
        }
        Inventory.Instance.MoveCoolDown = true
        mp.events.callRemote('MoveAllToSlotTrunk', vehId, oldSlot, newSlot)
        setTimeout(() => {
            Inventory.Instance.MoveCoolDown = false
        }, 500)
    }

    MoveItemToContainer(vehId: number, oldSlot: number, newSlot: number, fromInv: boolean) {
        if(Inventory.Instance.MoveCoolDown) {
            Notifications.Instance.DisplayNotify('{"Title":"Anti-Spam", "Message":"Du kannst nur 1 Item pro halber Sekunde bewegen.", "Time":3000}')
            Inventory.Instance.ShowComponent(false, '')
            return
        }
        Inventory.Instance.MoveCoolDown = true
        mp.events.callRemote('MoveItemToContainer', vehId, oldSlot, newSlot, fromInv)
        Inventory.Instance.ShowComponent(false, '')
        setTimeout(() => {
            Inventory.Instance.MoveCoolDown = false
        }, 500)
    }

    MoveAmountToSlot(oldSlot: number, newSlot: number, amount: number): void {
        if(Inventory.Instance.MoveCoolDown) {
            Notifications.Instance.DisplayNotify('{"Title":"Anti-Spam", "Message":"Du kannst nur 1 Item pro halber Sekunde bewegen.", "Time":3000}')
            Inventory.Instance.ShowComponent(false, '')
            return
        }
        Inventory.Instance.MoveCoolDown = true
        mp.events.callRemote('MoveAmountToSlot', oldSlot, newSlot, amount)
        setTimeout(() => {
            Inventory.Instance.MoveCoolDown = false
        }, 500)
    }

    MoveAmountToSlotTrunk(vehId: number, oldSlot: number, newSlot: number, amount: number) {
        if(Inventory.Instance.MoveCoolDown) {
            Notifications.Instance.DisplayNotify('{"Title":"Anti-Spam", "Message":"Du kannst nur 1 Item pro halber Sekunde bewegen.", "Time":3000}')
            Inventory.Instance.ShowComponent(false, '')
            return
        }
        Inventory.Instance.MoveCoolDown = true
        mp.events.callRemote('MoveAmountToSlotTrunk', vehId, oldSlot, newSlot, amount)
        setTimeout(() => {
            Inventory.Instance.MoveCoolDown = false
        }, 500)
    }
}

class Chat extends Windows {
    private static _instance = new Chat()
    public CoolDown: boolean
    private constructor() {
        super('Chat')
        this.CoolDown = false

        mp.events.add(ClientEvents.PlayerChat, this.PlayerChat)
        mp.keys.bind(0x54, false, this.ToggleChat)
    }

    public static get Instance(): Chat {
        return this._instance
    }

    public ToggleChat() {
        if(Chat.Instance.CoolDown || Chat.Instance.Visible || Login.Instance.Visible || Creator.Instance.Visible || Loading.Instance.Visible || Garage.Instance.Visible) return
        Chat.Instance.ShowComponent(true, '')
    }

    public override ShowComponent(show: boolean, data: string): void {
        this.Visible = show
        Browser.Instance.Execute(`window.vm.Chat.Show(${show}, '${data}')`)
        mp.gui.cursor.show(show, show)
    }

    public PlayerChat(data: string): void {
        Chat.Instance.ShowComponent(false, '')
        if(Chat.Instance.CoolDown) return
        Chat.Instance.CoolDown = true
        let args = data.split(' ')
        let event = args.shift()

        mp.events.callRemote(`CMD_${event}`, ...args)
        setTimeout(() => {
            Chat.Instance.CoolDown = false
        }, 1000)
    }
}

class Loading extends Windows {
    private static _instance = new Loading()
    private constructor() {
        super('Loading')

        mp.events.add(ClientEvents.LoadChangeData, this.ChangeData)
    }

    static get Instance(): Loading {
        return this._instance
    }

    public override ShowComponent(show: boolean, data: string): void {
        this.Visible = show
        Browser.Instance.Execute(`window.vm.Loading.Show(${show}, '${data}')`)
    }

    public ChangeData(data: string): void {
        Browser.Instance.Execute(`window.vm.Loading.ChangeData('${data}')`)
    }
}

class Notifications extends Windows {
    private static _instance = new Notifications()
    private AnnounceVisible: boolean
    private constructor() {
        super('Notifications')

        this.AnnounceVisible = false
        mp.events.add(ClientEvents.DisplayAnnounce, this.DisplayAnnounce)
        mp.events.add(ClientEvents.DisplayProgress, this.DisplayProgress)
        mp.events.add(ClientEvents.DisplayNotify, this.DisplayNotify)
    }

    static get Instance(): Notifications {
        return this._instance
    }

    public DisplayAnnounce(data: string): void {
        if(Notifications.Instance.AnnounceVisible) return
        Browser.Instance.Execute(`window.vm.Notifications.ShowAnnounce('${data}')`)
        Notifications.Instance.AnnounceVisible = true
        setTimeout(() => {
            Notifications.Instance.AnnounceVisible = false
        }, 7000)
    }

    public DisplayNotify(data: string): void {
        mp.game.audio.playSoundFrontend(1, "ATM_WINDOW", "HUD_FRONTEND_DEFAULT_SOUNDSET", true)
        Browser.Instance.Execute(`window.vm.Notifications.ShowNotify('${data}')`)
    }

    public DisplayProgress(data: string): void {
        mp.game.audio.playSoundFrontend(1, "ATM_WINDOW", "HUD_FRONTEND_DEFAULT_SOUNDSET", true)
        Browser.Instance.Execute(`window.vm.Notifications.ShowProgress('${data}')`)
        mp.console.logError(data)
    }
}

class Garage extends Windows {
    private static _instance = new Garage()
    private CoolDown: boolean
    private CurrentShape: ColshapeMp | null
    private constructor() {
        super('Garage')

        this.CurrentShape = null

        this.CoolDown = false
        mp.events.add('ParkVehicle', this.ParkVehicle)
        mp.events.add('TakeVehicle', this.TakeVehicle)
        mp.keys.bind(0x45, false, this.KeyPressE)
        mp.keys.bind(0x1B, false, this.PressEscape)
        mp.events.add('playerEnterColshape', this.EnterShape)
        mp.events.add("playerExitColshape", this.ExitShape);
    }

    static get Instance(): Garage {
        return this._instance
    }

    public EnterShape(shape: ColshapeMp) {
        if(shape == null || shape.getVariable("Type") != 'Garage') return

        Garage.Instance.CurrentShape = shape

        mp.game.audio.playSoundFrontend(1, "ATM_WINDOW", "HUD_FRONTEND_DEFAULT_SOUNDSET", true)
        Hud.Instance.DisplayInteraction('Drücke <span style="color: #078dbe">E</span> um mit der <span style="color: #078dbe">Garage</span> zu Interargieren.')
    }

    public ExitShape(shape: ColshapeMp) {
        if(shape == null || shape.getVariable("Type") != 'Garage') return

        Garage.Instance.CurrentShape = null

        Hud.Instance.HideInteraction()
    }

    public KeyPressE(): void {
        if(Garage.Instance.CurrentShape == null || Login.Instance.Visible || Creator.Instance.Visible || Inventory.Instance.Visible || Loading.Instance.Visible || Chat.Instance.Visible || Garage.Instance.Visible || ClothesShop.Instance.Visible || Shop.Instance.Visible) return

        mp.events.callRemote('OpenGarage', Garage.Instance.CurrentShape.getVariable('GarageId'))
    }

    public PressEscape(): void {
        if(!Garage.Instance.Visible) return
        Garage.Instance.ShowComponent(false, '')
    }

    public ParkVehicle(id: number): void {
        mp.events.callRemote('ParkVehicle', id)
        Garage.Instance.ShowComponent(false, '')
    }

    public TakeVehicle(garage:number, id: number): void {
        if(Garage.Instance.CoolDown) {
            Notifications.Instance.DisplayNotify('{"Title":"Anti-Spam", "Message":"Du kannst nur 1 Fahrzeug jede 5 Sekunden ausparken.", "Time":3000}')
            Garage.Instance.ShowComponent(false, '')
            return
        }
        mp.events.callRemote('TakeVehicle', garage, id)
        Garage.Instance.ShowComponent(false, '')
        Garage.Instance.CoolDown = true
        setTimeout(() => {
            Garage.Instance.CoolDown = false
        }, 3000)
    }
}

class Shop extends Windows {
    private static _instance = new Shop()
    private CurrentShape: ColshapeMp | null
    private constructor() {
        super('Shop')

        this.CurrentShape = null

        mp.events.add('BuyItems', this.BuyItems)
        mp.keys.bind(0x45, false, this.KeyPressE)
        mp.keys.bind(0x1B, false, this.PressEscape)
        mp.events.add('playerEnterColshape', this.EnterShape)
        mp.events.add("playerExitColshape", this.ExitShape);
    }

    static get Instance(): Shop {
        return this._instance
    }

    public EnterShape(shape: ColshapeMp) {
        if(shape == null || shape.getVariable("Type") != 'Shop') return

        Shop.Instance.CurrentShape = shape

        mp.game.audio.playSoundFrontend(1, "ATM_WINDOW", "HUD_FRONTEND_DEFAULT_SOUNDSET", true)
        Hud.Instance.DisplayInteraction('Drücke <span style="color: #078dbe">E</span> um mit dem <span style="color: #078dbe">Shop</span> zu Interargieren.')
    }

    public ExitShape(shape: ColshapeMp) {
        if(shape == null || shape.getVariable("Type") != 'Shop') return

        Shop.Instance.CurrentShape = null

        Hud.Instance.HideInteraction()
    }

    public KeyPressE(): void {
        if(Shop.Instance.CurrentShape == null || Login.Instance.Visible || Creator.Instance.Visible || Inventory.Instance.Visible || Loading.Instance.Visible || Chat.Instance.Visible || Garage.Instance.Visible || ClothesShop.Instance.Visible || Shop.Instance.Visible) return

        mp.events.callRemote('OpenShop', Shop.Instance.CurrentShape.getVariable('ShopId'))
    }

    public PressEscape(): void {
        if(!Shop.Instance.Visible) return
        Shop.Instance.ShowComponent(false, '')
    }

    public BuyItems(data: string): void {
        mp.events.callRemote('BuyItems', data)
        Shop.Instance.ShowComponent(false, '')
    }
}

class ClothesShop extends Windows {
    private static _instance = new ClothesShop()
    public OldPosition: Vector3Mp
    private CurrentShape: ColshapeMp | null
    private constructor() {
        super('ClothesShop')

        this.OldPosition = new mp.Vector3()
        this.CurrentShape = null

        mp.events.add('BuyClothes', this.BuyClothes)
        mp.events.add('ChangeClothes', this.ChangeClothes)
        mp.keys.bind(0x45, false, this.KeyPressE)
        mp.keys.bind(0x1B, false, this.PressEscape)
        mp.events.add('playerEnterColshape', this.EnterShape)
        mp.events.add("playerExitColshape", this.ExitShape);
    }

    static get Instance(): ClothesShop {
        return this._instance
    }

    public override ShowComponent(show: boolean, data: string): void {
        this.Visible = show
        Browser.Instance.Execute(`window.vm.ClothesShop.Show(${show}, '${data}')`)
        mp.gui.cursor.show(show, show)
        ControlsDisabled = show

        if(show){
            mp.players.local.freezePosition(true)
            mp.game.streaming.requestCollisionAtCoord(-168.83759, -298.70374, 39.73333)
            ClothesShop.Instance.OldPosition = mp.players.local.position
            let playerPos = new mp.Vector3(-168.83759, -298.70374, 39.73333)
            mp.players.local.position = playerPos
            mp.players.local.setRotation(0, 0, -83.14149, 1, true)
            sceneryCamera = mp.cameras.new('default', new mp.Vector3(-163.4897, -298.18713, 40.733337), new mp.Vector3(0,0,0), 42)
            sceneryCamera.pointAtCoord(playerPos.x, playerPos.y, playerPos.z)
            sceneryCamera.setActive(true)
            mp.game.cam.renderScriptCams(true, false, 0, true, false)
        } else {
            mp.events.callRemote('UpdateClothes')
            mp.players.local.freezePosition(false)
            mp.game.cam.renderScriptCams(false, false, 0, true, false)
            sceneryCamera.destroy()
            mp.players.local.position = ClothesShop.Instance.OldPosition
        }
        mp.game.ui.displayHud(!show)
        mp.game.ui.displayRadar(!show)
    }

    public EnterShape(shape: ColshapeMp) {
        if(shape == null || shape.getVariable("Type") != 'ClothesShop') return

        ClothesShop.Instance.CurrentShape = shape

        mp.game.audio.playSoundFrontend(1, "ATM_WINDOW", "HUD_FRONTEND_DEFAULT_SOUNDSET", true)
        Hud.Instance.DisplayInteraction('Drücke <span style="color: #078dbe">E</span> um mit dem <span style="color: #078dbe">Kleidungsladen</span> zu Interargieren.')
    }

    public ExitShape(shape: ColshapeMp) {
        if(shape == null || shape.getVariable("Type") != 'ClothesShop') return

        ClothesShop.Instance.CurrentShape = null

        Hud.Instance.HideInteraction()
    }

    public KeyPressE(): void {
        if(ClothesShop.Instance.CurrentShape == null || Login.Instance.Visible || Creator.Instance.Visible || Inventory.Instance.Visible || Loading.Instance.Visible || Chat.Instance.Visible || Garage.Instance.Visible || ClothesShop.Instance.Visible || Shop.Instance.Visible) return

        mp.events.callRemote('OpenClothesShop', ClothesShop.Instance.CurrentShape.getVariable('ShopId'))
    }

    public PressEscape(): void {
        if(!ClothesShop.Instance.Visible) return
        ClothesShop.Instance.ShowComponent(false, '')
    }

    public BuyClothes(cat: string, drawable: string, texture: string, price: string): void {
        mp.events.callRemote('BuyClothing', parseInt(cat), parseInt(drawable), parseInt(texture), parseInt(price))
        ClothesShop.Instance.ShowComponent(false, '')
    }

    public ChangeClothes(cat: string, drawable: string, texture: string){
        mp.players.local.setComponentVariation(parseInt(cat), parseInt(drawable), parseInt(texture), 2)
    }
}

class XMenu extends Windows {
    private static _instance = new XMenu()
    private RaycastCamera: CameraMp
    private activeHandle: EntityMp | null
    private constructor() {
        super('XMenu')

        this.RaycastCamera = mp.cameras.new("gameplay")
        this.activeHandle = null

        mp.keys.bind(0x58, true, this.OpenMenu)
        mp.keys.bind(0x58, false, this.CloseMenu)
        mp.events.add('SelectXMenuItem', this.SelectItem)
    }

    static get Instance(): XMenu {
        return this._instance
    }

    public OpenMenu() {
        if(PlayerDead || Login.Instance.Visible || Creator.Instance.Visible || Garage.Instance.Visible || Chat.Instance.Visible || Shop.Instance.Visible || ClothesShop.Instance.Visible) return;
        
        let player = mp.players.local
        if(!PlayerCuffed && player.isInAnyVehicle(false)){
            Browser.Instance.Execute(`window.vm.XMenu.setDataItems('[{"label": "Exit", "description": "Das Menü schließen.", "icon": "exit", "id": "1", "arg": "exit"},{"label": "Auf/Zuschließen", "description": "Das Fahrzeug öffnen/schließen.", "icon": "lock", "id": "2", "arg": "lock"},{"label": "Kofferraum", "description": "Den Kofferraum öffnen/schließen.", "icon": "trunk", "id": "3", "arg": "trunk"},{"label": "Motor", "description": "Den Motor an/abschalten.", "icon": "engine", "id": "4", "arg": "engine"},{"label": "Radio", "description": "Das Radio ausschalten.", "icon": "radio", "id": "5", "arg": "radio"}]')`)
            mp.gui.cursor.show(true, true)
            ControlsDisabled = true
            XMenu.Instance.activeHandle = player.vehicle
            return
        } else {
            let obj = XMenu.Instance.createRaycast();

            if (obj != null) {
                let distance = mp.game.gameplay.getDistanceBetweenCoords(player.position.x, player.position.y, player.position.z, obj.position.x, obj.position.y, obj.position.z, true);

                if (!PlayerCuffed && obj.entity.isAVehicle()) {
                    if (!distance || distance < 0 || distance > 3) return;

                    Browser.Instance.Execute(`window.vm.XMenu.setDataItems('[{"label": "Exit", "description": "Das Menü schließen.", "icon": "exit", "id": "1", "arg": "exit"},{"label": "Auf/Zuschließen", "description": "Das Fahrzeug öffnen/schließen.", "icon": "lock", "id": "2", "arg": "lock"},{"label": "Kofferraum", "description": "Den Kofferraum öffnen/schließen.", "icon": "trunk", "id": "3", "arg": "trunk"},{"label": "Information", "description": "Fahrzeug Inforamtionen.", "icon": "info", "id": "4", "arg": "vehicleinfo"},{"label": "Reparieren", "description": "Das Fahrzeug Reparieren.", "icon": "repair", "id": "5", "arg": "repair"}]')`)
                    mp.gui.cursor.show(true, true)
                    ControlsDisabled = true
                    XMenu.Instance.activeHandle = obj.entity
                    return
                } else if (obj.entity.isAPed()) {
                    if (!distance || distance < 0 || distance > 2) {
                        return;
                    }

                    if(PlayerCuffed){
                        Browser.Instance.Execute(`window.vm.XMenu.setDataItems('[{"label": "Exit", "description": "Das Menü schließen.", "icon": "exit", "id": "1", "arg": "exit"},{"label": "Support Info", "description": "Support Info von einem Spieler erhalten.", "icon": "info", "id": "2", "arg": "playerinfo"}]')`)
                        mp.gui.cursor.show(true, true)
                        ControlsDisabled = true
                        XMenu.Instance.activeHandle = obj.entity
                        return
                    }

                    Browser.Instance.Execute(`window.vm.XMenu.setDataItems('[{"label": "Exit", "description": "Das Menü schließen.", "icon": "exit", "id": "1", "arg": "exit"},{"label": "Geld Geben", "description": "Einem Spieler Geld geben.", "icon": "givemoney", "id": "2", "arg": "givemoney"},{"label": "Ausweis Geben", "description": "Einem Spieler deinen Ausweis geben.", "icon": "idcard", "id": "3", "arg": "giveidcard"},{"label": "Ausweis Nehmen", "description": "Von einem Spieler den Ausweis nehmen.", "icon": "idcard", "id": "4", "arg": "takeidcard"},{"label": "Fesseln", "description": "Einem Spieler fesseln anlegen/abnehmen.", "icon": "seile", "id": "5", "arg": "cuffplayer"},{"label": "Item Geben", "description": "Einem Spieler ein Item geben.", "icon": "giveitem", "id": "6", "arg": "giveitem"},{"label": "Support Info", "description": "Support Info von einem Spieler erhalten.", "icon": "info", "id": "7", "arg": "playerinfo"}]')`)
                    mp.gui.cursor.show(true, true)
                    ControlsDisabled = true
                    XMenu.Instance.activeHandle = obj.entity
                    return
                }
            }
        }
    }

    public CloseMenu() {
        Browser.Instance.Execute(`window.vm.XMenu.setDataItems('[]')`)
        mp.gui.cursor.show(false, false)
        ControlsDisabled = false
    }

    public SelectItem(id: number, arg: string) {
        mp.gui.cursor.show(false, false)
        ControlsDisabled = false
        if(XMenu.Instance.activeHandle == null) return
        switch (arg) {
            /* VEHICLE */
            case 'lock':
                mp.events.callRemote('XMenu_LockVehicle', XMenu.Instance.activeHandle)
                break;
            case 'trunk':
                mp.events.callRemote('XMenu_LockTrunk', XMenu.Instance.activeHandle)
                break;
            case 'engine':
                mp.events.callRemote('XMenu_ToggleEngine', XMenu.Instance.activeHandle)
                break;
            case 'radio':
                mp.game.audio.setRadioToStationName("OFF")
                break;
            case 'repair':
                mp.events.callRemote('XMenu_RepairVehicle', XMenu.Instance.activeHandle)
                break;
            case 'vehicleinfo':
                mp.events.callRemote('XMenu_GetVehicleInfo', XMenu.Instance.activeHandle)
                break;
            /* PLAYER */
            case 'playerinfo':
                mp.events.callRemote('XMenu_GetPlayerInfo', XMenu.Instance.activeHandle)
                break;
            case 'givemoney':
                InputDialog.Instance.ShowComponent(true, `{"CallBack":"GiveMoney", "ItemImage":"money.png"}`)
                InputDialog.Instance.TargetEntity = XMenu.Instance.activeHandle
                mp.gui.cursor.show(true, true)
                break;
            case 'giveidcard':
                mp.events.callRemote('XMenu_GivePlayerIdcard', XMenu.Instance.activeHandle)
                break;
            case 'takeidcard':
                mp.events.callRemote('XMenu_TakePlayerIdcard', XMenu.Instance.activeHandle)
                break;
            case 'cuffplayer':
                mp.events.callRemote('XMenu_CuffPlayer', XMenu.Instance.activeHandle)
                break;
            case 'giveitem':
                break;
        }
        XMenu.Instance.activeHandle = null
    }

    private getCameraHitCoord(): RaycastResult | null {
        let position = XMenu.Instance.RaycastCamera.getCoord();
        let direction = XMenu.Instance.RaycastCamera.getDirection();
        let farAway = new mp.Vector3(direction.x * 12 + position.x, direction.y * 12 + position.y, direction.z * 12 + position.z);
        let hitData = mp.raycasting.testPointToPoint(position, farAway, mp.players.local);

        if (hitData != undefined) {
            return hitData;
        }
        return null;
    }

    private createRaycast(): RaycastResult | null {
        let obj = this.getCameraHitCoord();
        if (obj == null) {
            mp.gui.chat.push("no obj found");
        } else {
            if (obj.entity == null || obj.entity == undefined) return null;
            if (obj.entity.handle == null || obj.entity.handle == undefined) return null;

            let entityCheck = mp.game.entity.isAnEntity(obj.entity.handle);
            if (entityCheck) {
                return obj;
            }

            return null;
        }
        return null;
    }
}

class InputDialog extends Windows {
    private static _instance = new InputDialog()
    public TargetEntity: EntityMp | null
    private constructor() {
        super('InputDialog')

        this.TargetEntity = null
        
        mp.events.add('GiveMoney', this.GiveMoney)
    }

    static get Instance(): InputDialog {
        return this._instance
    }

    public GiveMoney(input: string) {
        InputDialog.Instance.ShowComponent(false, '')
        if(!InputDialog.Instance.isNumeric(input) || InputDialog.Instance.TargetEntity == null) return
        let money = parseInt(input)
        if(money < 1) return

        mp.events.callRemote('GivePlayerMoney', money, InputDialog.Instance.TargetEntity)
        InputDialog.Instance.TargetEntity = null
    }

    private isNumeric(str: string): boolean {
        if (typeof str != "string") return false
        return !isNaN(parseFloat(str))
    }
}

let ControlsDisabled = false
let PlayerCuffed = false

function DisableAllControls(state: boolean) {
    ControlsDisabled = state
}

mp.events.add('SetCuffState', (state: boolean) => {
    PlayerCuffed = state
    ControlsDisabled = state
    mp.players.local.setEnableHandcuffs(state)
})
mp.events.add('StartDeathScreen', () => {
    PlayerInvincible = true
    mp.game.graphics.startScreenEffect('DeathFailOut', 1000, true)
    DisableAllControls(true)
    PlayerDead = true
})
mp.events.add('DisableDeathScreen', () => {
    Browser.Instance.ShowComponent('Loading', true, 'Du wirst wiederbelebt...')
    setTimeout(() => {
        Browser.Instance.ShowComponent('Loading', true, 'Wiederbelebung erfolgreich.')
        setTimeout(() => {
            mp.players.local.setInvincible(false)
            mp.game.graphics.stopScreenEffect('DeathFailOut');
            Browser.Instance.ShowComponent('Loading', false, '')
            DisableAllControls(false)
            PlayerDead = false
            PlayerInvincible = false
        }, 750)
    }, 2000)
})

let PlayerInvincible = true;
let Aduty = false;

mp.events.add('render', () => {
    if(ControlsDisabled){
        mp.game.player.disableFiring(true)
        mp.game.controls.disableControlAction(0, 22, true) //Space
        mp.game.controls.disableControlAction(0, 23, true) //Veh Enter
        mp.game.controls.disableControlAction(0, 25, true) //Right Mouse
        mp.game.controls.disableControlAction(0, 44, true) //Q
        mp.game.controls.disableControlAction(2, 75, true) //Exit Vehicle
        mp.game.controls.disableControlAction(2, 140, true) //R
        mp.game.controls.disableControlAction(2, 141, true) //Left Mouse
        mp.game.controls.disableControlAction(0, 30, true) //Move LR
        mp.game.controls.disableControlAction(0, 31, true) //Move UD
    }

    mp.game.ui.displayAmmoThisFrame(false)
    mp.game.ped.setAiWeaponDamageModifier(0.2)
    mp.players.local.setSuffersCriticalHits(false)
    mp.game.stats.statSetInt(mp.game.joaat('SP0_STAMINA'), 100, false)
    mp.game.stats.statSetInt(mp.game.joaat('SP0_SHOOTING_ABILITY'), 100, false)
    mp.game.stats.statSetInt(mp.game.joaat('SP0_STRENGTH'), 100, false)
    mp.game.stats.statSetInt(mp.game.joaat('SP0_STEALTH_ABILITY'), 100, false)
    mp.game.stats.statSetInt(mp.game.joaat('SP0_FLYING_ABILITY'), 100, false)
    mp.game.stats.statSetInt(mp.game.joaat('SP0_WHEELIE_ABILITY'), 100, false)
    mp.game.stats.statSetInt(mp.game.joaat('SP0_LUNG_CAPACITY'), 100, false)
    mp.game.player.setHealthRechargeMultiplier(0.0)
    mp.players.local.setInvincible(PlayerInvincible)
    mp.game.ui.hideHudComponentThisFrame(7); // area name
    mp.game.ui.hideHudComponentThisFrame(9); // street name
    mp.game.ui.hideHudComponentThisFrame(8);
    mp.game.ui.hideHudComponentThisFrame(6);
    mp.game.ui.hideHudComponentThisFrame(3);
    mp.game.ui.hideHudComponentThisFrame(5);
    mp.players.local.setConfigFlag(32, false);
    mp.players.local.setConfigFlag(35, false);
    mp.players.local.setConfigFlag(52, false);
    mp.players.local.setConfigFlag(429, false);
    if(mp.players.local.vehicle){
        mp.players.local.vehicle.setEnginePowerMultiplier(16);
        mp.players.local.vehicle.setEngineTorqueMultiplier(1.2);
    }

    mp.players.forEachInStreamRange(x => {
        x.setVisible(x.getVariable("Visible"), false)
    })

    mp.players.forEachInRange(mp.players.local.position, 30, x => {
        if(!x.getVariable("ADUTY") || !x.getVariable("Visible")) return;

        mp.game.graphics.drawText(x.getVariable("ADMINRANK"), [x.position.x, x.position.y, x.position.z+1.2], { 
            font: 0,
            color: [255, 0, 0, 255],
            scale: [0.35, 0.35],
            outline: true,
            centre: true
        });
    })
})

mp.keys.bind(0x77, false, () => {
    mp.gui.cursor.show(!mp.gui.cursor.visible, !mp.gui.cursor.visible)
})

mp.events.add('ADUTY', (state: boolean) => {
    Aduty = state;
    PlayerInvincible = state;
    Hud.Instance.SetAduty(state);
    mp.nametags.enabled = state;
})

let NoClip = false;
let noClipCamera: any;
let shiftModifier = false;
let controlModifier = false;
mp.keys.bind(0x74, false, () => {
    if(!Aduty) return;
    if(!NoClip){
        NoClip = true;
        mp.events.callRemote('SetVisible', false)
        var camPos = new mp.Vector3(
        mp.players.local.position.x,
        mp.players.local.position.y,
        mp.players.local.position.z
        );
        var camRot = mp.game.cam.getGameplayCamRot(2);
        noClipCamera = mp.cameras.new('default', camPos, camRot, 45);
        noClipCamera.setActive(true);
        mp.game.cam.renderScriptCams(true, false, 0, true, false);
        mp.players.local.freezePosition(true);
        mp.players.local.setCollision(false, false);
    } else {
        NoClip = false;
        mp.events.callRemote('SetVisible', true)
        if (noClipCamera) {
        mp.players.local.position = noClipCamera.getCoord();
        mp.players.local.setHeading(noClipCamera.getRot(2).z);
        noClipCamera.destroy(true);
        noClipCamera = null;
        }
        mp.game.cam.renderScriptCams(false, false, 0, true, false);
        mp.players.local.freezePosition(false);
        mp.players.local.setCollision(true, false);
    }
})

var getNormalizedVector = function(vector: Vector3Mp) {
    var mag = Math.sqrt(
      vector.x * vector.x + vector.y * vector.y + vector.z * vector.z
    );
    vector.x = vector.x / mag;
    vector.y = vector.y / mag;
    vector.z = vector.z / mag;
    return vector;
};
  var getCrossProduct = function(v1: Vector3Mp, v2: Vector3Mp) {
    var vector = new mp.Vector3(0, 0, 0);
    vector.x = v1.y * v2.z - v1.z * v2.y;
    vector.y = v1.z * v2.x - v1.x * v2.z;
    vector.z = v1.x * v2.y - v1.y * v2.x;
    return vector;
};
mp.events.add('render', function() {
    if (!noClipCamera) return;
    controlModifier = mp.keys.isDown(17);
    shiftModifier = mp.keys.isDown(16);
    var rot = noClipCamera.getRot(2);
    var fastMult = 1;
    var slowMult = 1;
    if (shiftModifier) {
      fastMult = 3;
    } else if (controlModifier) {
      slowMult = 0.5;
    }
    var rightAxisX = mp.game.controls.getDisabledControlNormal(0, 220);
    var rightAxisY = mp.game.controls.getDisabledControlNormal(0, 221);
    var leftAxisX = mp.game.controls.getDisabledControlNormal(0, 218);
    var leftAxisY = mp.game.controls.getDisabledControlNormal(0, 219);
    var pos = noClipCamera.getCoord();
    var rr = noClipCamera.getDirection();
    var vector = new mp.Vector3(0, 0, 0);
    vector.x = rr.x * leftAxisY * fastMult * slowMult;
    vector.y = rr.y * leftAxisY * fastMult * slowMult;
    vector.z = rr.z * leftAxisY * fastMult * slowMult;
    var upVector = new mp.Vector3(0, 0, 1);
    var rightVector = getCrossProduct(
      getNormalizedVector(rr),
      getNormalizedVector(upVector)
    );
    rightVector.x *= leftAxisX * 0.5;
    rightVector.y *= leftAxisX * 0.5;
    rightVector.z *= leftAxisX * 0.5;
    var upMovement = 0.0;
    if (mp.keys.isDown(69)) {
      upMovement = 0.5;
    }
    var downMovement = 0.0;
    if (mp.keys.isDown(81)) {
      downMovement = 0.5;
    }
    mp.players.local.position = new mp.Vector3(
      pos.x + vector.x + 1,
      pos.y + vector.y + 1,
      pos.z + vector.z + 1
    );
    mp.players.local.heading = rr.z;
    noClipCamera.setCoord(
      pos.x - vector.x + rightVector.x,
      pos.y - vector.y + rightVector.y,
      pos.z - vector.z + rightVector.z + upMovement - downMovement
    );
    noClipCamera.setRot(
      rot.x + rightAxisY * -5.0,
      0.0,
      rot.z + rightAxisX * -5.0,
      2
    );
});