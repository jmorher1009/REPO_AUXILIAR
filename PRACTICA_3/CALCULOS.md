# Práctica 3 – Transistores: cálculos y resultados

Resultados de simulación obtenidos con LTspice 26.1.1 (BC547B, modelo `standard.bjt`).

## A. Curva característica (`A_curva.asc`)

`.dc V2 0 6 0.01 I1 0 150u 25u` → 7 curvas (IB = 0, 25, 50, …, 150 µA).

Recta de carga con Rc = 150 Ω: `Ic = (6 − Vce)/150`  (corta en 40 mA con Vce = 0 y en 6 V con Ic = 0).

Valores de las curvas en Vce = 3 V:

| IB (µA) | 0 | 25 | 50 | 75 | 100 | 125 | 150 |
|---|---|---|---|---|---|---|---|
| Ic (mA) | 0 | 7,03 | 13,46 | 19,42 | 24,99 | 30,25 | 35,24 |

**A.4 – Punto de trabajo Vce = 3 V**
- a) Ic = (6 − 3)/150 = **20 mA**
- b) La recta de carga cae justo por encima de la curva IB = 75 µA (19,4 mA); interpolando entre 75 y 100 µA: **IB ≈ 77 µA** (≈ 75 µA leyendo la gráfica).
- c) β = Ic/IB = 20 mA / 77 µA ≈ **260** (≈ 257; ≈ 267 si se usa IB = 75 µA).

## B. Zonas de trabajo (`B_zonas.asc` (V1 = 1,5 V, 0 V y 3 V))

Circuito: V1 – R1 = 10 kΩ – base; Rc(R2) = 150 Ω; V2 = 6 V.

**B.1 – V1 para el punto de A.4** (IB = 77 µA, VBE ≈ 0,7 V en activa):

V1 = IB·R1 + VBE = 77 µA · 10 kΩ + 0,7 V ≈ **1,5 V**

**B.2 – Intervalos**

- **Corte**: VBE < 0,7 V (V1 < 0,7 V) → IB = 0, IC = 0, VCE = V2 = 6 V.
- **Saturación**: VCE = VCEsat = 0,2 V
  - IC,sat = (6 − 0,2)/150 = 38,7 mA
  - IB,sat = IC,sat/β = 38,7 mA / 257 ≈ 150 µA
  - V1,sat = IB,sat·R1 + VBEsat = 150 µA · 10 kΩ + 0,8 V ≈ 2,3 V → saturación para **V1 ≥ 2,3 V**
- **Activa**: **0,7 V < V1 < 2,3 V**; IC = β·IB, VCE = 6 − 150·IC.

| V1 | VBE | VCE | IC | IB | Zona |
|---|---|---|---|---|---|
| 1,5 V (0,7–2,3 V) | 0,7 V | 3 V | 20 mA | 77 µA | Activa |
| 0 V (< 0,7 V) | 0 V | 6 V | 0 | 0 | Corte |
| 3 V (≥ 2,3 V) | 0,8 V | 0,2 V | 38,7 mA | (3−0,8)/10k = 220 µA | Saturación |

**B.3 – Simulación `.op` (LTspice)**

| V1 | VBE | VCE | IC | IB | Zona |
|---|---|---|---|---|---|
| 1,5 V | 0,728 V | 3,01 V | 19,92 mA | 77 µA | Activa |
| 0 V | 0 V | 6 V | 0 | 0 | Corte |
| 3 V | 0,759 V | 0,190 V | 38,74 mA | 224 µA | Saturación |

## C. Transistor como interruptor (`C_interruptor.asc`)

Datos: β = 10, VCEsat = 0,2 V, VBEsat = 0,8 V, VLED = 2 V, V2 = 12 V, V1 = 5 V.

**C.1 – Rc**: Ic = (V2 − VLED − VCEsat)/Rc = 9,8 V / Rc
- Ic = 20 mA → Rc = 490 Ω
- Ic = 10 mA → Rc = 980 Ω
- → Rc ∈ [490, 980] Ω. Elegimos **Rc = 680 Ω** (valor comercial) → Ic = 9,8/680 = 14,4 mA.

**C.1 – Rb**: para saturar, IB ≥ Ic/β = 14,4 mA/10 = 1,44 mA
- Rb ≤ (V1 − VBEsat)/IB = (5 − 0,8)/1,44 mA = 2,9 kΩ
- Si se parte de Ic = 10…20 mA: IB = 1…2 mA → Rb ∈ [2,1; 4,2] kΩ
- Elegimos **Rb = 2,2 kΩ** → IB = 4,2/2,2k = 1,9 mA (> 1,44 mA, satura con margen).

**C.2 – Simulación** (`PULSE(0 5 0 1u 1u 4m 10m)`, `.tran 30m`, 3 periodos)

| Estado | V1 | IC | VC | IB |
|---|---|---|---|---|
| Encendido (0–4 ms) | 5 V | 14,5 mA | 0,05 V | 1,94 mA |
| Apagado (4–10 ms) | 0 V | ≈ 0 | ≈ 11 V (LED casi sin corriente) | 0 |

IC simulada = 14,5 mA ∈ [10, 20] mA ✔.

Gráficas a visualizar (tres paneles): `V(n003)` (V1), `Ic(Q1)`, `V(vc)`.

## Montaje (tabla a rellenar en el laboratorio)
Usar V1 = 1,5 V / 0 V / 3 V (mismos valores que B.2) y medir VBE, VCE, IB (mediante V sobre R1: IB = (V1 − VB)/10 kΩ) e IC (= (6 − VC)/150 Ω).
