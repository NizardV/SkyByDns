"use client";

import { motion } from "motion/react";
import { useNavigate } from 'react-router-dom';
import { cn } from "@/lib/utils";
import { Globe, Shield, Zap, Server, Lock, Activity } from "lucide-react";

function DNSShape({
  className,
  delay = 0,
  width = 400,
  height = 100,
  rotate = 0,
  gradient = "from-blue-500/[0.08]",
  borderRadius = 16,
}: {
  className?: string;
  delay?: number;
  width?: number;
  height?: number;
  rotate?: number;
  gradient?: string;
  borderRadius?: number;
}) {
  return (
    <motion.div
      animate={{
        opacity: 1,
        y: 0,
        rotate,
      }}
      className={cn("absolute", className)}
      initial={{
        opacity: 0,
        y: -150,
        rotate: rotate - 15,
      }}
      transition={{
        duration: 2.4,
        delay,
        ease: [0.23, 0.86, 0.39, 0.96],
        opacity: { duration: 1.2 },
      }}
    >
      <motion.div
        animate={{
          y: [0, 15, 0],
        }}
        className="relative"
        style={{
          width,
          height,
        }}
        transition={{
          duration: 12,
          repeat: Number.POSITIVE_INFINITY,
          ease: "easeInOut",
        }}
      >
        <div
          className={cn(
            "absolute inset-0",
            "bg-linear-to-r to-transparent",
            gradient,
            "backdrop-blur-[1px]",
            "ring-1 ring-white/[0.03] dark:ring-white/[0.02]",
            "shadow-[0_2px_16px_-2px_rgba(255,255,255,0.04)]",
            "after:absolute after:inset-0",
            "after:bg-[radial-gradient(circle_at_50%_50%,rgba(255,255,255,0.12),transparent_70%)]",
            "after:rounded-[inherit]"
          )}
          style={{ borderRadius }}
        />
      </motion.div>
    </motion.div>
  );
}

interface DNSNode {
  id: number;
  x: number;
  y: number;
  size: number;
  delay: number;
  pulseDelay: number;
}

export default function DNSHero({
  title1 = "Global DNS",
  title2 = "Management",
  subtitle = "Enterprise-grade DNS management with real-time analytics, DDoS protection, and global edge network",
}: {
  title1?: string;
  title2?: string;
  subtitle?: string;
}) {
  const navigate = useNavigate();
  const fadeUpVariants = {
    hidden: { opacity: 0, y: 30 },
    visible: (i: number) => ({
      opacity: 1,
      y: 0,
      transition: {
        duration: 1,
        delay: 0.5 + i * 0.2,
        ease: [0.25, 0.4, 0.25, 1],
      },
    }),
  };

  const dnsNodes: DNSNode[] = [
    { id: 1, x: 10, y: 15, size: 4, delay: 0.1, pulseDelay: 0 },
    { id: 2, x: 25, y: 40, size: 6, delay: 0.3, pulseDelay: 1 },
    { id: 3, x: 45, y: 25, size: 5, delay: 0.2, pulseDelay: 2 },
    { id: 4, x: 65, y: 60, size: 7, delay: 0.4, pulseDelay: 3 },
    { id: 5, x: 80, y: 30, size: 5, delay: 0.5, pulseDelay: 4 },
    { id: 6, x: 35, y: 70, size: 4, delay: 0.6, pulseDelay: 5 },
    { id: 7, x: 60, y: 15, size: 6, delay: 0.7, pulseDelay: 6 },
    { id: 8, x: 15, y: 55, size: 5, delay: 0.8, pulseDelay: 7 },
    { id: 9, x: 75, y: 75, size: 4, delay: 0.9, pulseDelay: 8 },
    { id: 10, x: 90, y: 50, size: 7, delay: 1.0, pulseDelay: 9 },
  ];

  const features = [
    { icon: Zap, label: "100ms Global TTL", color: "text-blue-400" },
    { icon: Shield, label: "DDoS Protection", color: "text-green-400" },
    { icon: Globe, label: "300+ Edge Locations", color: "text-purple-400" },
    { icon: Lock, label: "DNSSEC", color: "text-amber-400" },
    { icon: Activity, label: "Real-time Analytics", color: "text-rose-400" },
    { icon: Server, label: "Anycast Network", color: "text-cyan-400" },
  ];

  return (
    <div className="relative flex min-h-screen w-[100vw] items-center justify-center overflow-hidden bg-gradient-to-br from-slate-950 via-blue-950/30 to-slate-950">
      {/* Network connections */}
      <div className="absolute inset-0">
        {dnsNodes.map((node, idx) => (
          <motion.div
            key={`line-${node.id}`}
            className="absolute h-[1px] bg-gradient-to-r from-blue-500/10 to-cyan-500/10"
            initial={{ width: 0, opacity: 0 }}
            animate={{ width: "100%", opacity: 0.3 }}
            transition={{ delay: node.delay + 0.5, duration: 1.5 }}
            style={{
              left: `${dnsNodes[idx].x}%`,
              top: `${dnsNodes[idx].y}%`,
              transform: `rotate(${Math.random() * 360}deg)`,
            }}
          />
        ))}
      </div>

      {/* DNS nodes */}
      <div className="absolute inset-0">
        {dnsNodes.map((node) => (
          <motion.div
            key={node.id}
            className="absolute rounded-full bg-gradient-to-br from-blue-500/20 to-cyan-500/20"
            initial={{ scale: 0, opacity: 0 }}
            animate={{ scale: 1, opacity: 0.6 }}
            transition={{ delay: node.delay, duration: 0.8 }}
            style={{
              left: `${node.x}%`,
              top: `${node.y}%`,
              width: `${node.size}px`,
              height: `${node.size}px`,
            }}
          >
            <motion.div
              className="absolute inset-0 rounded-full border border-blue-400/30"
              animate={{
                scale: [1, 2, 1],
                opacity: [0.5, 0, 0.5],
              }}
              transition={{
                duration: 3,
                delay: node.pulseDelay * 0.3,
                repeat: Infinity,
                ease: "easeInOut",
              }}
            />
          </motion.div>
        ))}
      </div>

      {/* Background shapes */}
      <div className="absolute inset-0 overflow-hidden">
        <DNSShape
          borderRadius={24}
          className="top-[-10%] left-[-15%]"
          delay={0.3}
          gradient="from-blue-500/[0.15] dark:from-blue-500/[0.25]"
          height={500}
          rotate={-8}
          width={300}
        />

        <DNSShape
          borderRadius={20}
          className="right-[-20%] bottom-[-5%]"
          delay={0.5}
          gradient="from-cyan-500/[0.15] dark:from-cyan-500/[0.25]"
          height={200}
          rotate={15}
          width={600}
        />

        <DNSShape
          borderRadius={32}
          className="top-[40%] left-[-5%]"
          delay={0.4}
          gradient="from-indigo-500/[0.15] dark:from-indigo-500/[0.25]"
          height={300}
          rotate={24}
          width={300}
        />
      </div>

      {/* Global network map overlay */}
      <div className="absolute inset-0 opacity-10">
        <div className="h-full w-full bg-[radial-gradient(circle_at_50%_50%,rgba(59,130,246,0.1),transparent_70%)]" />
      </div>

      <div className="container relative z-10 mx-auto px-4 md:px-6">
        <div className="mx-auto max-w-6xl">
          <motion.div
            animate="visible"
            custom={1}
            initial="hidden"
            variants={fadeUpVariants as any}
            className="mb-8 text-center"
          >
            <div className="mb-6 inline-flex items-center gap-2 rounded-full border border-blue-500/20 bg-blue-500/10 px-4 py-2 backdrop-blur-sm">
              <div className="h-2 w-2 animate-pulse rounded-full bg-green-400" />
              <span className="text-sm font-medium text-blue-300">
                Trusted by 1M+ domains worldwide
              </span>
            </div>
          </motion.div>

          <motion.div
            animate="visible"
            custom={1}
            initial="hidden"
            variants={fadeUpVariants as any}
            className="mb-8 text-center"
          >
            <h1 className="mb-6 font-bold text-4xl tracking-tight sm:text-6xl md:mb-8 md:text-7xl lg:text-8xl">
              <span className="bg-linear-to-b from-blue-100 to-blue-300 bg-clip-text text-transparent">
                {title1}
              </span>
              <br />
              <span className="bg-linear-to-r from-cyan-300 via-blue-300 to-indigo-400 bg-clip-text text-transparent">
                {title2}
              </span>
            </h1>
          </motion.div>

          <motion.div
            animate="visible"
            custom={2}
            initial="hidden"
            variants={fadeUpVariants as any}
            className="mb-12 text-center"
          >
            <p className="mx-auto max-w-2xl text-xl text-slate-300 leading-relaxed">
              {subtitle}
            </p>
          </motion.div>

          {/* Feature chips */}
          <motion.div
            animate="visible"
            custom={3}
            initial="hidden"
            variants={fadeUpVariants as any}
            className="mb-12"
          >
            <div className="flex flex-wrap justify-center gap-4">
              {features.map((feature, index) => (
                <motion.div
                  key={feature.label}
                  initial={{ opacity: 0, y: 20 }}
                  animate={{ opacity: 1, y: 0 }}
                  transition={{ delay: 0.8 + index * 0.1 }}
                  className="group flex items-center gap-2 rounded-full border border-white/10 bg-white/5 px-4 py-2 backdrop-blur-sm transition-all hover:bg-white/10 hover:border-white/20"
                >
                  <feature.icon className={`h-4 w-4 ${feature.color}`} />
                  <span className="text-sm font-medium text-slate-200">
                    {feature.label}
                  </span>
                </motion.div>
              ))}
            </div>
          </motion.div>

          {/* Check Dns With input */}
          <motion.div
            animate="visible"
            custom={4}
            initial="hidden"
            variants={fadeUpVariants as any}
            className="mb-12 flex items-center justify-center gap-2"
          >
            <input
              type="text"
              placeholder="Enter your domain (e.g. example.com)"
              className="w-full max-w-sm rounded-lg border border-white/20 bg-white/5 px-4 py-2 text-slate-200 backdrop-blur-sm focus:border-blue-500 focus:ring-1 focus:ring-blue-500 focus:outline-none"
            />
            <button
              className="rounded-lg bg-gradient-to-r from-blue-600 to-cyan-600 px-6 py-2 font-semibold text-white shadow-lg shadow-blue-500/25 transition-colors duration-200 hover:from-blue-700 hover:to-cyan-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2"
            >
              Check
            </button>
          </motion.div>

          {/* CTA Buttons */}
          <motion.div
            animate="visible"
            custom={4}
            initial="hidden"
            variants={fadeUpVariants as any}
            className="flex flex-col items-center justify-center gap-4 sm:flex-row"
          >
            <motion.button
              whileHover={{ scale: 1.05 }}
              whileTap={{ scale: 0.95 }}
              className="group relative overflow-hidden rounded-lg bg-gradient-to-r from-blue-600 to-cyan-600 px-8 py-3 font-semibold text-white shadow-lg shadow-blue-500/25"
              onClick={() => navigate('/dashboard')}
            >
              <span className="relative z-10">Get Started Free</span>
              <div className="absolute inset-0 bg-gradient-to-r from-blue-700 to-cyan-700 opacity-0 transition-opacity group-hover:opacity-100" />
            </motion.button>

            <motion.button
              whileHover={{ scale: 1.05 }}
              whileTap={{ scale: 0.95 }}
              className="rounded-lg border border-white/20 bg-white/5 px-8 py-3 font-semibold text-white backdrop-blur-sm transition-all hover:bg-white/10"
            >
              Schedule Demo
            </motion.button>
          </motion.div>

          {/* Stats */}
          <motion.div
            animate="visible"
            custom={5}
            initial="hidden"
            variants={fadeUpVariants as any}
            className="mt-20 grid grid-cols-2 gap-6 md:grid-cols-4"
          >
            {[
              { value: "99.99%", label: "Uptime SLA" },
              { value: "<50ms", label: "Avg Response Time" },
              { value: "300+", label: "Edge Locations" },
              { value: "10Tbps", label: "DDoS Protection" },
            ].map((stat, index) => (
              <div
                key={stat.label}
                className="text-center"
              >
                <div className="mb-2 text-3xl font-bold text-blue-300">
                  {stat.value}
                </div>
                <div className="text-sm text-slate-400">{stat.label}</div>
              </div>
            ))}
          </motion.div>
        </div>
      </div>

      {/* Gradient overlays */}
      <div className="pointer-events-none absolute inset-0 bg-gradient-to-t from-slate-950 via-transparent to-slate-950/80" />
      <div className="pointer-events-none absolute inset-0 bg-[radial-gradient(ellipse_at_center,rgba(59,130,246,0.05),transparent_50%)]" />
    </div>
  );
}