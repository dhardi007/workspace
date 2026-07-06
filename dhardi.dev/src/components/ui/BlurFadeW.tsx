import { BlurFade } from "./blur-fade";
import type { ReactNode } from "react";

interface Props {
  children: ReactNode;
  className?: string;
  delay?: number;
}

export default function BlurFadeW(props: Props) {
  return <BlurFade {...props} />;
}
