import{_ as x,r as i,o as V,w as A,a as v,c as u,b as l,f as b,v as L,F as E,g as R,t as c,h as q,i as F,j,k as _,l as B,d as N,e as C}from"./index-BNLlVnfH.js";const O={name:"HardwareReservation",setup(){const m=i([]),a=i({}),h=i({}),e=i(null),g=i([]),w=i([]),t=i(""),o=i(""),S=i([]),y=i({}),H=async()=>{try{const[n,r,f]=await Promise.all([fetch("hardware"),fetch("/reservation/eventsForReservation"),fetch("/reservation/allReservations")]);m.value=await n.json();const d=await r.json();S.value=await f.json(),d.forEach(s=>{const D=new Date(s.eventStartDate),k=new Date(s.eventEndDate);s.eventStartDate=D.toLocaleDateString("de-DE"),s.eventEndDate=k.toLocaleDateString("de-DE")}),g.value=d,m.value.forEach(s=>{y.value[s.id]=s.totalAmount})}catch(n){console.error("Error fetching hardware data:",n)}},M=async()=>{if(w.value.length>0){const n=w.value.filter(r=>a.value[r]>0).map(r=>({hardwareId:r,quantity:a.value[r]}));if(n.length>0)try{const r={eventId:e.value.eventId,hardwareList:n},d=await(await fetch("/reservation",{method:"POST",headers:{"Content-Type":"application/json"},body:JSON.stringify(r)})).json();d.success?(t.value=d.message,o.value=""):(o.value=d.message,t.value="")}catch(r){o.value="Fehler bei der Serververbindung.",console.error("Error:",r)}else o.value="Bitte wählen Sie Hardware mit Menge zur Reservierung aus.",t.value=""}else o.value="Bitte wählen Sie Hardware aus, um eine Reservierung zu machen."},p=()=>{const n={};m.value.forEach(r=>{n[r.id]=r.totalAmount}),e.value&&S.value.forEach(r=>{const f=new Date(r.eventStartDate);r.eventStartDate=f.toLocaleDateString("de-DE"),r.eventStartDate===e.value.eventStartDate&&r.hardwareList&&r.hardwareList.forEach(d=>{const s=d.hardwareId,D=d.quantity;n[s]!==void 0&&(n[s]=n[s]-D)})}),y.value=n};return V(()=>{H().then(()=>{p()})}),A(e,()=>{p()}),{hardwareOptions:m,selectedHardware:w,selectedEvent:e,eventsForReservation:g,hardwareId:h,quantity:a,reserveHardware:M,successMsg:t,errorMsg:o,hardwareAvailability:y}}},T=["value"],U={key:0},I=["value"],z=["onUpdate:modelValue","max","disabled"],P={key:0,class:"success-message"},J={key:1,class:"error-message"};function W(m,a,h,e,g,w){return v(),u(E,null,[a[5]||(a[5]=l("h2",null,"Reservierung",-1)),a[6]||(a[6]=l("p",null,"Achtung: Die Reservierung darf nur mindestens 4 Woche vor Veranstaltungsdurchführung getätigt werden.",-1)),l("form",{onSubmit:a[2]||(a[2]=B((...t)=>e.reserveHardware&&e.reserveHardware(...t),["prevent"]))},[l("div",null,[a[3]||(a[3]=l("label",{for:"myEvent"},"Bitte wählen Sie ein Event aus der Liste aus. ",-1)),b(l("select",{"onUpdate:modelValue":a[0]||(a[0]=t=>e.selectedEvent=t),id:"myEvent",required:""},[(v(!0),u(E,null,R(e.eventsForReservation,t=>(v(),u("option",{key:t.eventId,value:t},c(t.eventName)+" - ( "+c(t.eventStartDate)+" bis "+c(t.eventEndDate)+" ) ",9,T))),128))],512),[[L,e.selectedEvent]])]),e.hardwareAvailability?(v(),u("div",U,[(v(!0),u(E,null,R(e.hardwareOptions,t=>(v(),u("div",{key:t.id},[l("label",null,[b(l("input",{type:"checkbox",value:t.id,"onUpdate:modelValue":a[1]||(a[1]=o=>e.selectedHardware=o)},null,8,I),[[q,e.selectedHardware]]),F(" "+c(t.name)+" ("+c(e.hardwareAvailability[t.id]||0)+" verfügbar) ",1),b(l("input",{"onUpdate:modelValue":o=>e.quantity[t.id]=o,max:e.hardwareAvailability[t.id],type:"number",min:"0",disabled:e.hardwareAvailability[t.id]<=0},null,8,z),[[j,e.quantity[t.id]]])])]))),128)),a[4]||(a[4]=l("button",{type:"submit"},"Reservieren",-1))])):_("",!0)],32),e.successMsg?(v(),u("div",P,c(e.successMsg),1)):e.errorMsg?(v(),u("div",J,c(e.errorMsg),1)):_("",!0)],64)}const G=x(O,[["render",W]]),Q=N({__name:"ReservationView",setup(m){return(a,h)=>(v(),u("main",null,[C(G)]))}});export{Q as default};
