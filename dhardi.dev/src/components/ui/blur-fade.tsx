import { motion, useInView } from "motion/react";
import { type ReactNode, useRef } from "react";

interface Props {
  children: ReactNode;
  className?: string;
  delay?: number;
  direction?: "up" | "down" | "left" | "right";
  duration?: number;
  blur?: number;
  once?: boolean;
}

export function BlurFade({
  children,
  className = "",
  delay = 0,
  direction = "up",
  duration = 0.5,
  blur: blurAmount = 6,
  once = true,
}: Props) {
  const ref = useRef<HTMLDivElement>(null);
  const inView = useInView(ref, { once });
  const offsets: Record<string, { x: number; y: number }> = {
    up: { x: 0, y: 20 },
    down: { x: 0, y: -20 },
    left: { x: 20, y: 0 },
    right: { x: -20, y: 0 },
  };
  const offset = offsets[direction] || offsets.up;

  return (
    <motion.div
      ref={ref}
      className={className}
      initial={{ opacity: 0, filter: `blur(${blurAmount}px)`, x: offset.x, y: offset.y }}
      animate={inView ? { opacity: 1, filter: "blur(0px)", x: 0, y: 0 } : {}}
      transition={{ duration, delay, ease: "easeOut" }}
    >
      {children}
    </motion.div>
  );
}
